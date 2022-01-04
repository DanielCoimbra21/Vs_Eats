﻿using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IDishesRestaurantDB
    {

        int GetIdRestaurant(int idDish);

        List<int> GetListDishes(int idRestaurant);

    }
}
