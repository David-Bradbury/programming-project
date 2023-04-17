using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProgrammingProject.Helper
{
    public class DropDownLists
    {
        public static List<string> GetStates()
        {
            List<string> statesList = new List<string>
            {
                "South Australia",
                "Victoria",
                "Western Australia",
                "Northern Territory",
                "New South Wales",
                "Australian Capital Territory",
                "Queensland",
                "Tasmania"
            };
            return statesList;
        }

        public static List<SelectListItem> GetInsuranceList() {

            List<SelectListItem> InsuranceList = new List<SelectListItem>();
            
            InsuranceList.Add(new SelectListItem { Text = "Yes, I have insurance", Value = "true" });
            InsuranceList.Add(new SelectListItem { Text = "No, I do not have insurance", Value = "false" });

            return InsuranceList;
        }

        public static List<SelectListItem> GetExperienceLevel()
        {
            List<SelectListItem> ExperienceList = new List<SelectListItem>();

            ExperienceList.Add(new SelectListItem { Text = "Beginner", Value = "1" });
            ExperienceList.Add(new SelectListItem { Text = "Intermediate", Value = "2" });
            ExperienceList.Add(new SelectListItem { Text = "Advanced", Value = "3" });
            ExperienceList.Add(new SelectListItem { Text = "Expert", Value = "4" });

            return ExperienceList;
        }
    }
}
