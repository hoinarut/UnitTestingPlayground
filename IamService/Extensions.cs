using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IamService
{
    public static class Extensions
    {
        public static Dictionary<string, string> AsDictionary(this ModelStateDictionary errors)
        {
            var dict = new Dictionary<string, string>();
            foreach (var key in errors.Keys.Where(k => errors.Any(e => e.Key == k && e.Value.Errors.Any())).ToList())
            {
                dict.Add(key, string.Join(",", errors[key].Errors.Select(e => e.ErrorMessage)));
            }

            return dict;
        }
    }
}
