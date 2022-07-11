﻿using System;
using System.Linq;
using System.Collections.Generic;

namespace SalesWeb.Models
{
    public class Seller
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public double BaseSalary { get; set; }
        public Department Department { get; set; }
        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

        public Seller()
        {
        }

        public Seller(int id, string name, string email, DateTime birthDate, double baseSalary, Department department)
        {
            Id = id;
            Name = name;
            Email = email;
            BirthDate = birthDate;
            BaseSalary = baseSalary;
            Department = department;
        }

        public void AddSales(SalesRecord sr)
        {
            if (sr != null)
                Sales.Add(sr);
        }

        public void RemoveSales(SalesRecord sr)
        {
            if (sr != null)
                Sales.Remove(sr);
        }

        public double TotalSales(DateTime initial, DateTime final)
        {
            return (from s in Sales where s.Date >= initial && s.Date <= final select s.Amount).Sum();
        }
    }
}
