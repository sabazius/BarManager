﻿using BarManager.Models.DTO;
using BarManager.Models.Requests;
using System.Collections.Generic;

namespace BarManager.BL.Interfaces
{
    public interface IBillService
    {
        Bill Create(Bill bill);

        Bill Update(Bill bill);

        Bill Delete(int id);

        Bill GetById(int id);

        IEnumerable<Bill> GetAll();
       
    }
}
