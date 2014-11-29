using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Mobowski.Core.Sports {
    public class CachingWebClient : WebClient  {
        public SportManagerBase SportManager { get; private set; }

        public CachingWebClient(SportManagerBase sportManager) : base() {
            this.SportManager = sportManager;
        }
    }
}
