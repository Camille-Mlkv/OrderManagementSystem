﻿namespace OrderService.Application.DTOs.Address
{
    public class AddressRequestDto
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
