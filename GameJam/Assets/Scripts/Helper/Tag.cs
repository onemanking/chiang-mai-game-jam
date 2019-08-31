using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Enum

public enum TagType
{
    None,
    Officer,
    Prisoner,
}

#endregion


namespace Helper
{
    public struct Tag
    {
        public const string m_sOfficer = "Officer";
        public const string m_sPrisoner = "Prisoner";

        public static TagType GetTagType(string sTagName)
        {
            switch (sTagName)
            {
                case m_sOfficer:
                    return TagType.Officer;

                case m_sPrisoner:
                    return TagType.Prisoner;
            }

            return TagType.None;
        }
    }
}

#region Extension

public static class extension_Tag
{
    public static string String(this TagType eType)
    {
        switch (eType)
        {
            case TagType.Officer:
                return Helper.Tag.m_sOfficer;

            case TagType.Prisoner:
                return Helper.Tag.m_sPrisoner;
        }

        return null;
    }
}


#endregion