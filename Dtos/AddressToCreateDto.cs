﻿namespace CodeFirst.Dtos
{
    public class AddressToCreateDto
    {
        public required string Street { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public required string PostalCode { get; set; }
    }
}
