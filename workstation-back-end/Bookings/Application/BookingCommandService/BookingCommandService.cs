using FluentValidation;
using workstation_back_end.Bookings.Domain;
using workstation_back_end.Bookings.Domain.Models.Commands;
using workstation_back_end.Bookings.Domain.Models.Entities;
using workstation_back_end.Bookings.Domain.Services;
using workstation_back_end.Experience.Domain.Models.Queries;
using workstation_back_end.Experience.Domain.Services;
using workstation_back_end.Shared.Domain.Repositories;

namespace workstation_back_end.Bookings.Application.BookingCommandService;

public class BookingCommandService : IBookingCommandService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExperienceQueryService _experienceQueryService;
    private readonly IValidator<CreateBookingCommand> _validator;

    public BookingCommandService(IBookingRepository bookingRepository, IUnitOfWork unitOfWork, IExperienceQueryService experienceQueryService, IValidator<CreateBookingCommand> validator)
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _experienceQueryService = experienceQueryService;
        _validator = validator;
    }
    
       public async Task<Booking?> Handle(CreateBookingCommand command)
    {
        var experience = await _experienceQueryService.Handle(new GetExperienceByIdQuery(command.ExperienceId));
        if (experience == null) 
        {

            throw new ArgumentException("Experience not found for the given ID."); 
        }
        
        var validationResult = await _validator.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var totalPrice = experience.Price * command.NumberOfPeople;

        var booking = new Booking(
            command.TouristId, // 
            command.ExperienceId,
            command.BookingDate.Date,
            command.NumberOfPeople,
            totalPrice,
            "Confirmada", 
            command.time
        );

        try
        {
            await _bookingRepository.AddAsync(booking);
            await _unitOfWork.CompleteAsync();
            return booking;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error al crear reserva en repositorio: {e.Message}");

            throw; 
         
        }
    }

    public async Task<Booking?> Handle(UpdateBookingStatusCommand command)
    {
        var existingBooking = await _bookingRepository.FindByIdAsync(command.BookingId);
        if (existingBooking is null) return null; 

        existingBooking.Status = command.NewStatus;

        try
        {
            _bookingRepository.Update(existingBooking);
            await _unitOfWork.CompleteAsync();
            return existingBooking;
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred while updating the booking status: {e.Message}");
            return null;
        }
    }

    public async Task<bool> Handle(DeleteBookingCommand command)
    {
        var existingBooking = await _bookingRepository.FindByIdAsync(command.BookingId);
        if (existingBooking is null) return false; 

        try
        {
            _bookingRepository.Remove(existingBooking);
            await _unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred while deleting the booking: {e.Message}");
            return false;
        }
    }
}