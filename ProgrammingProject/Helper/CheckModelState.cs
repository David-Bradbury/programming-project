using Microsoft.AspNetCore.Mvc;
using ProgrammingProject.Data;
using ProgrammingProject.Models;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProgrammingProject.Helper;
using Microsoft.AspNetCore.WebUtilities;
using System.Web.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace ProgrammingProject.Helper
{
    public class CheckModelState
    {
        private readonly EasyWalkContext _context;
        public CheckModelState(EasyWalkContext context)
        {
            _context = context;
        }

        //Testing to see if this is required.
        public string IsSuburbDataUnique(string SuburbName, string Postcode, string State)
        {

            var sName = _context.Suburbs.Where(s => s.SuburbName == SuburbName);
            if (sName.IsNullOrEmpty())
                return "SuburbNameFail";

            var sPostcode = _context.Suburbs.Where(s => s.Postcode == Postcode);
            if (sPostcode.IsNullOrEmpty())
                return "PostcodeFail";

            var sNamePostcodeMatch = _context.Suburbs.Where(s => s.SuburbName == SuburbName)
                                                     .Where(s => s.Postcode == Postcode);
            if (sNamePostcodeMatch.IsNullOrEmpty())
                return "SuburbNamePostcodeFail";

            var Suburb = _context.Suburbs.Where(s => s.SuburbName == SuburbName)
                                       .Where(s => s.Postcode == Postcode)
                                       .Where(s => s.State == State);
            if (Suburb.IsNullOrEmpty())
                return "Fail";

            return "Pass";

        }

    }
}
