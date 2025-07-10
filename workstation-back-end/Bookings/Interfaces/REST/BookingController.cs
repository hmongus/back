using System.Net.Mime;
using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using workstation_back_end.Bookings.Domain.Models.Commands;
using workstation_back_end.Bookings.Domain.Models.Queries;
using workstation_back_end.Bookings.Domain.Services;
using workstation_back_end.Bookings.Interfaces.REST.Transform;

namespace workstation_back_end.Bookings.Interfaces.REST
{
    /// <summary>
    /// API Controller for managing bookings.
    /// It allows creating, querying, and deleting bookings for tourists and agencies.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class BookingController : ControllerBase
    {
        private readonly IBookingCommandService _bookingCommandService;
        private readonly IBookingQueryService _bookingQueryService;

        public BookingController(
            IBookingCommandService bookingCommandService,
            IBookingQueryService bookingQueryService)
        {
            _bookingCommandService = bookingCommandService;
            _bookingQueryService = bookingQueryService;
        }

        /// <summary>
        /// Creates a new booking.
        /// </summary>
        /// <remarks>
        /// Note: The `CreateBookingCommand` class must have properties that match this JSON structure.
        /// <br/>
        /// Sample request:
        ///
        ///     POST /api/v1/Booking
        ///     {
        ///        "touristId": "a1b2c3d4-e5f6-7890-1234-567890abcdef",
        ///        "experienceId": 1,
        ///        "bookingDate": "2025-06-20T00:00:00.000Z",
        ///        "numberOfPeople": 2,
        ///     }
        ///
        /// </remarks>
        /// <param name="command">The command object to create a booking.</param>
        /// <response code="201">Returns the newly created booking.</response>
        /// <response code="400">If the command is invalid or the booking could not be created.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
         public async Task<IActionResult> CreateBooking([FromBody] CreateBookingCommand command)
        {
            try
            {
                // 1. Obtener el ID del user (TouristId) del token JWT
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null)
                {
                    // Esto indica un problema con el token o la configuración de autenticación
                    return Unauthorized("User ID claim not found in token. Please log in again.");
                }

                Guid touristId;
                if (!Guid.TryParse(userIdClaim.Value, out touristId))
                {
                    return BadRequest("Invalid user ID format in authentication token.");
                }
                
                var commandWithTouristId = command with { TouristId = touristId }; 

                // 3. Pasar el comando modificado al servicio
                var booking = await _bookingCommandService.Handle(commandWithTouristId); 
                
                if (booking is null) 
                {
                    return BadRequest("The reservation could not be created due to an unknown issue or invalid data.");
                }

                var resource = BookingAssembler.ToResourceFromEntity(booking);
                return CreatedAtAction(nameof(GetBookingById), new { bookingId = resource.Id }, resource);
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors.Select(error => new
                {
                    field = error.PropertyName,
                    message = error.ErrorMessage
                });

                return BadRequest(new
                {
                    message = "Validation failed.",
                    errors = errors
                });
            }
            catch (ArgumentException ex) // Para "Experience not found" u otros ArgumentException
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (MySqlException ex) // <--- Manejo específico para errores de MySQL
            {
                Console.WriteLine($"Database error when creating a reservation: {ex.Message}");
                // Puedes ser más específico aquí si quieres manejar diferentes errores de DB
                if (ex.Message.Contains("FOREIGN KEY constraint fails") && ex.Message.Contains("FK_Bookings_Users_TouristId"))
                {
                    return BadRequest(new { message = "The tourist specified for the booking does not exist. Please ensure you are logged in with a valid tourist account." });
                }
                // Si hay otro error de FK (ej. ExperienceId no existe)
                else if (ex.Message.Contains("FOREIGN KEY constraint fails") && ex.Message.Contains("FK_Bookings_Experiences_ExperienceId"))
                {
                     return BadRequest(new { message = "The experience specified for the booking does not exist." });
                }
                return Problem(detail: "A database error occurred while creating the booking.", statusCode: StatusCodes.Status500InternalServerError);
            }
            catch (ApplicationException ex) // Para errores controlados de tu dominio (como el "Could not create the booking due to a database error.")
            {
                Console.WriteLine($"Application error when creating a reservation: {ex.Message}");
                // En producción, podrías querer un mensaje más genérico para el cliente.
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex) // Para cualquier otra excepción inesperada
            {
                Console.WriteLine($"Unexpected error creating reservation: {ex.Message}");
                return Problem(detail: "An internal server error occurred.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }



        /// <summary>
        /// Gets a booking by its ID.
        /// </summary>
        /// <param name="bookingId">The booking ID.</param>
        /// <response code="200">Returns the requested booking.</response>
        /// <response code="404">If the booking with the specified ID does not exist.</response>
        [HttpGet("{bookingId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBookingById(int bookingId)
        {
            var query = new GetBookingByIdQuery(bookingId);
            var booking = await _bookingQueryService.Handle(query);
            if (booking is null)
                return NotFound($"Booking with ID {bookingId} not found.");

            var resource = BookingAssembler.ToResourceFromEntity(booking);
            return Ok(resource);
        }

        /// <summary>
        /// Gets all bookings for a specific tourist.
        /// </summary>
        /// <param name="touristId">The tourist's ID (Guid).</param>
        /// <response code="200">Returns the list of bookings for the tourist.</response>
        /// <response code="404">If no bookings are found for the specified tourist.</response>
        [HttpGet("tourist/{touristId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBookingsByTourist(Guid touristId)
        {
            var query = new GetBookingsByTouristIdQuery(touristId); 
            var bookings = await _bookingQueryService.Handle(query);
            var resources = bookings.Select(BookingAssembler.ToResourceFromEntity);
            return Ok(resources);
        }

        /// <summary>
        /// Deletes a booking by its ID.
        /// </summary>
        /// <param name="bookingId">The ID of the booking to delete.</param>
        /// <response code="204">If the booking was deleted successfully.</response>
        /// <response code="404">If the booking with the specified ID does not exist.</response>
        [HttpDelete("{bookingId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBooking(int bookingId)
        {
            var command = new DeleteBookingCommand(bookingId);
            var result = await _bookingCommandService.Handle(command);
            if (!result)
                return NotFound($"Booking with ID {bookingId} not found.");

            return NoContent();
        }
        
        /// <summary>
        /// Gets all bookings.
        /// </summary>
        /// <response code="200">Returns the list of all bookings.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBookings()
        {
            var query = new GetAllBookingsQuery();
            var bookings = await _bookingQueryService.Handle(query); 
            var resources = bookings.Select(BookingAssembler.ToResourceFromEntity); 
    
            return Ok(resources);
        }
    }
}