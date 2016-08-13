﻿using DartApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartApp.CommandServices
{
    public interface IDartAppCommandService
    {
	    void InitializeDatabase(string setup, string mappingPath, string testValueFile);
		void InsertPlayer(Player newPlayer);
		void UpdatePlayer(Player newPlayer);
		void DeletePlayer(Player playerToDelete);
		void AddToHoliday(Player newPlayer);
		void RemoveFromHoliday(Player playerToRemove);
    }
}
