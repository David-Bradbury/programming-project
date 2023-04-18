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
    }
}
