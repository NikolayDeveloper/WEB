using System.Collections.Generic;

namespace Iserv.Niis.Domain.Helpers
{
    public static class ObjectType
    {
        public static List<int> GetRequestRouteIds()
        {
            return new List<int>
            {
                DdWorktypeId.B,
                DdWorktypeId.TM,
                DdWorktypeId.SA,
                DdWorktypeId.S2,
                DdWorktypeId.U,
                DdWorktypeId.NMPT,
                DdWorktypeId.TMI,
                DdWorktypeId.AP,
                DdWorktypeId.B_1,
                DdWorktypeId.TIM
            };
        }

        public static List<int> GetMaterilsRouteIds()
        {
            return new List<int>
            {
                DdWorktypeId.IN,
                DdWorktypeId.OUT,
                DdWorktypeId.W,
                DdWorktypeId.INТТТ
            };
        }

        public static List<int> GetProtectionDocRouteIds()
        {
            return new List<int>
            {
                DdWorktypeId.GR,
            };
        }

        public static List<int> GetContractRouteIds()
        {
            return new List<int>
            {
                //DdWorktypeId.DK_GR,
                DdWorktypeId.DK
            };
        }
    }
}