using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Utils
{
    public static class Extensions
    {
        public const string AdminClaim = "admin";

        public static string Error(this ModelStateDictionary modelState)
        {
            foreach (var key in modelState.Keys)
            {
                if (modelState[key].Errors.Count > 0)
                    return modelState[key].Errors[0].ErrorMessage;
            }
            return string.Empty;
        }
    }
}
