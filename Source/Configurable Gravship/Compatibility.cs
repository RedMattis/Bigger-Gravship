using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace GravshipSize.Compat
{
    public static class GravshipsExpanded
    {
        public static bool IsActive => ModsConfig.IsActive("vanillaexpanded.gravship");
    }
}
