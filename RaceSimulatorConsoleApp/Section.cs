using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Section
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int[] Direction { get; set; }

        public SectionTypes SectionType { get; set; }

        public Section(SectionTypes sectionType)
        {
            SectionType = sectionType;
        }
    }

    public enum SectionTypes
    {
        Straight,
        LeftCorner,
        RightCorner,
        StartGrid,
        Finish
    }
}
