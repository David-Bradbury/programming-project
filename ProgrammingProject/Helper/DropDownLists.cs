using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProgrammingProject.Helper
{
    public class DropDownLists
    {
        public static List<SelectListItem> GetStates()
        {
            List<SelectListItem> statesList = new List<SelectListItem>();

                statesList.Add(new SelectListItem { Text = "South Australia", Value = "South Australia" });
                statesList.Add(new SelectListItem { Text = "Victoria", Value = "Victoria" });
                statesList.Add(new SelectListItem { Text = "Western Australia", Value = "Western Australia" });
                statesList.Add(new SelectListItem { Text = "Northern Territory", Value = "Northern Territory" });
                statesList.Add(new SelectListItem { Text = "New South Wales", Value = "New South Wales" });
                statesList.Add(new SelectListItem { Text = "Australian Capital Territory", Value = "Australian Capital Territory" });
                statesList.Add(new SelectListItem { Text = "Queensland", Value = "Queensland" });
                statesList.Add(new SelectListItem { Text = "Tasmania", Value = "Tasmania" });
                      
            return statesList;
        }

        public static List<SelectListItem> GetInsuranceList() {

            List<SelectListItem> InsuranceList = new List<SelectListItem>();
            
            InsuranceList.Add(new SelectListItem { Text = "Yes, I have insurance", Value = "Insured" });
            InsuranceList.Add(new SelectListItem { Text = "No, I do not have insurance", Value = "Uninsured" });

            return InsuranceList;
        }

        public static List<SelectListItem> GetExperienceLevel()
        {
            List<SelectListItem> ExperienceList = new List<SelectListItem>();

            ExperienceList.Add(new SelectListItem { Text = "Beginner", Value = "Beginner" });
            ExperienceList.Add(new SelectListItem { Text = "Intermediate", Value = "Intermediate" });
            ExperienceList.Add(new SelectListItem { Text = "Advanced", Value = "Advanced" });
            ExperienceList.Add(new SelectListItem { Text = "Expert", Value = "Expert" });

            return ExperienceList;
        }
        public static List<SelectListItem> GetDogSize()
        {
            List<SelectListItem> DogSizeList = new List<SelectListItem>();

            DogSizeList.Add(new SelectListItem { Text = "Small", Value = "Small" });
            DogSizeList.Add(new SelectListItem { Text = "Medium", Value = "Medium" });
            DogSizeList.Add(new SelectListItem { Text = "Large", Value = "Large" });
            DogSizeList.Add(new SelectListItem { Text = "ExtraLarge", Value = "ExtraLarge" });

            return DogSizeList;
        }
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
        public static List<SelectListItem> GetTrainingLevel()
        {
            List<SelectListItem> TrainingLevel = new List<SelectListItem>();

            TrainingLevel.Add(new SelectListItem { Text = "None", Value = "None" });
            TrainingLevel.Add(new SelectListItem { Text = "Basic", Value = "Basic" });
            TrainingLevel.Add(new SelectListItem { Text = "Fully", Value = "Fully" });       

            return TrainingLevel;
        }
        public static List<SelectListItem> GetVaccinatedList()
        {

            List<SelectListItem> VaccinatedList = new List<SelectListItem>();

            VaccinatedList.Add(new SelectListItem { Text = "Yes, the dog is vaccinated", Value = "Vaccinated" });
            VaccinatedList.Add(new SelectListItem { Text = "No, the dog is not vaccinated", Value = "Unvaccinated" });

            return VaccinatedList;
        }
    }
}
