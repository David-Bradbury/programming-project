﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProgrammingProject.Helper
{
    public class DropDownLists
    {
        /*
         * All lists created here are for ViewModels to parse to views to create drop down boxes.
         */

        // Fills list full of Australian states and territories.
        public static List<SelectListItem> GetStates()
        {
            List<SelectListItem> statesList = new List<SelectListItem>();

            statesList.Add(new SelectListItem { Text = "South Australia", Value = "SA" });
            statesList.Add(new SelectListItem { Text = "Victoria", Value = "VIC" });
            statesList.Add(new SelectListItem { Text = "Western Australia", Value = "WA" });
            statesList.Add(new SelectListItem { Text = "Northern Territory", Value = "NT" });
            statesList.Add(new SelectListItem { Text = "New South Wales", Value = "NSW" });
            statesList.Add(new SelectListItem { Text = "Australian Capital Territory", Value = "ACT" });
            statesList.Add(new SelectListItem { Text = "Queensland", Value = "QLD" });
            statesList.Add(new SelectListItem { Text = "Tasmania", Value = "TAS" });

            return statesList;
        }

        // Fills list of insurance status for walkers.
        public static List<SelectListItem> GetInsuranceList()
        {

            List<SelectListItem> InsuranceList = new List<SelectListItem>();

            InsuranceList.Add(new SelectListItem { Text = "Yes, I have insurance", Value = "Insured" });
            InsuranceList.Add(new SelectListItem { Text = "No, I do not have insurance", Value = "Uninsured" });

            return InsuranceList;
        }

        // Fills list of walkers experience levels.
        public static List<SelectListItem> GetExperienceLevel()
        {
            List<SelectListItem> ExperienceList = new List<SelectListItem>();

            ExperienceList.Add(new SelectListItem { Text = "Beginner", Value = "Beginner" });
            ExperienceList.Add(new SelectListItem { Text = "Intermediate", Value = "Intermediate" });
            ExperienceList.Add(new SelectListItem { Text = "Advanced", Value = "Advanced" });
            ExperienceList.Add(new SelectListItem { Text = "Expert", Value = "Expert" });

            return ExperienceList;
        }

        // Fills list of different dog sizes when creating / editing a dog.
        public static List<SelectListItem> GetDogSize()
        {
            List<SelectListItem> DogSizeList = new List<SelectListItem>();

            DogSizeList.Add(new SelectListItem { Text = "Small - 2-9 Kg", Value = "Small" });
            DogSizeList.Add(new SelectListItem { Text = "Medium - 9-18 Kg", Value = "Medium" });
            DogSizeList.Add(new SelectListItem { Text = "Large - 18-30 Kg", Value = "Large" });
            DogSizeList.Add(new SelectListItem { Text = "ExtraLarge - 30+ Kg", Value = "ExtraLarge" });

            return DogSizeList;
        }

        // Fills list of different dog temperament states when creating / editing a dog.
        public static List<SelectListItem> GetTemperament()
        {
            List<SelectListItem> TemperamentList = new List<SelectListItem>();

            TemperamentList.Add(new SelectListItem { Text = "NonReactive", Value = "NonReactive" });
            TemperamentList.Add(new SelectListItem { Text = "Calm", Value = "Calm" });
            TemperamentList.Add(new SelectListItem { Text = "Friendly", Value = "Friendly" });
            TemperamentList.Add(new SelectListItem { Text = "Reactive", Value = "Reactive" });
            TemperamentList.Add(new SelectListItem { Text = "Aggresive", Value = "Agressive" });

            return TemperamentList;
        }

        // Fills list of different dog training levels used when creating / editing a dog.
        public static List<SelectListItem> GetTrainingLevel()
        {
            List<SelectListItem> TrainingLevel = new List<SelectListItem>();

            TrainingLevel.Add(new SelectListItem { Text = "None", Value = "None" });
            TrainingLevel.Add(new SelectListItem { Text = "Basic", Value = "Basic" });
            TrainingLevel.Add(new SelectListItem { Text = "Fully", Value = "Fully" });

            return TrainingLevel;
        }

        // Fills list of vaccination status of dogs.
        public static List<SelectListItem> GetVaccinatedList()
        {

            List<SelectListItem> VaccinatedList = new List<SelectListItem>();

            VaccinatedList.Add(new SelectListItem { Text = "Yes, the dog is fully vaccinated", Value = "Vaccinated" });
            VaccinatedList.Add(new SelectListItem { Text = "No, the dog is not fully vaccinated", Value = "Unvaccinated" });

            return VaccinatedList;
        }
    }
}
