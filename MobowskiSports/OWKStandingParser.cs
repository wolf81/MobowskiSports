using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Mobowski.Core.Sports
{
  class OWKStandingParser : IParser<Standing>
  {

    public void Parse(Standing t, object data)
    {
      JToken json = (JToken)data;
      t.Team = (string)json.SelectToken("team_name");
    }
  }
}
