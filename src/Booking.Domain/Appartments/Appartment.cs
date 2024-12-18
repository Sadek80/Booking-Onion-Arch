using Booking.Domain.Abstractions;
using Booking.Domain.Appartments.Enums;
using Booking.Domain.Appartments.ValueObjects;
using Booking.Domain.Shared.ValueObjects;

namespace Booking.Domain.Appartments
{
    public sealed class Appartment : Entity<AppartmentId>
    {
        public Appartment(AppartmentId Id,
                          Name name,
                          Address address,
                          Description description,
                          Money price,
                          Money cleaningFee,
                          List<Amenity> amenities,
                          DateTime? lastBookedOnUTC)
            : base(Id)
        {
            Name = name;
            Address = address;
            Description = description;
            Price = price;
            CleaningFee = cleaningFee;
            Amenities = amenities;
            LastBookedOnUTC = lastBookedOnUTC;
        }

        private Appartment()
        {
        }

        public Name Name { get; private set; }
        public Address Address { get; private set; }
        public Description Description { get; private set; }
        public Money Price { get; private set; }
        public Money CleaningFee { get; private set; }
        public List<Amenity> Amenities { get; private set; } = new();
        public DateTime? LastBookedOnUTC { get; internal set; }
    }
}
