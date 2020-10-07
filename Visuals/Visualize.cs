using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using Controller;
using Model;

namespace Visuals
{
    public static class Visualize
    {
        #region paths
        public const string _finish = ".//images/track_tiles/track_finish.png";
        public const string _straight = ".//images/track_tiles/track_straight.png";
        public const string _start = ".//images/track_tiles/track_startgrid.png";
        public const string _corner = ".//images/track_tiles/track_corner.png";

        public const string _carRed = ".//images/cars/car_red.png";
        public const string _carBlue = ".//images/cars/car_blue.png";
        public const string _carGrey = ".//images/cars/car_grey.png";
        public const string _carYellow = ".//images/cars/car_yellow.png";
        public const string _carGreen = ".//images/cars/car_green.png";

        #endregion

        #region settings

        public static int visualWidth = 100;
        public static int visualHeight = 100;

        #endregion

        public static BitmapSource DrawTrack(Track track)
        {
            int[] size = GetTrackWidthHeight(track);
            Bitmap empty = ImageLoader.CreateBitmapFromSize(size[0] * visualWidth, size[1] * visualHeight);
            Graphics g = Graphics.FromImage(empty);

            foreach (Section section in track.Sections)
            {
                Image visualSection = GetSectionVisual(section.Direction, section.SectionType);
                int startX = section.X * visualWidth;
                int startY = section.Y * visualHeight + 1;

                g.DrawImage(visualSection, new Point(startX, startY));
            }

            foreach (Section section in track.Sections)
            {
                SectionData sectionData = Data.CurrentRace.GetSectionData(section);
                
                if (sectionData.Left != null)
                    DrawParticipant(g, section, (Driver)sectionData.Left);
                if (sectionData.Right != null)
                    DrawParticipant(g, section, (Driver)sectionData.Right);

            }

            //empty.SetResolution(1000,1000);

            return ImageLoader.CreateBitmapSourceFromGdiBitmap(empty);
        }

        public static void DrawParticipant(Graphics g, Section section, Driver driver)
        {
            SectionData data = Data.CurrentRace.GetSectionData(section);

            Point pt = GetParticipantCoords(section);
            int[][] offset = GetSectionOffsets(section.SectionType, section.Direction, (data.Left == driver ? data.DistanceLeft : data.DistanceRight), (data.Left == driver));

            pt.X += offset[0][0];
            pt.Y += offset[0][1];

            Bitmap map = GetParticipantBitmap(driver.TeamColor, offset[1]);

            g.DrawImage(map, pt);

            if (driver.Equipment.IsBroken)
            {
                DrawFire(g, pt, driver.FireCount, offset[1]);
                driver.FireCount++;
            }
        }

        public static void DrawFire(Graphics g, Point pt, int fireIndex, int[] direction)
        {
            Bitmap map = ImageLoader.GetBitmap($".//images/fire/fire_{fireIndex + 1}.png");

            if (direction[0] == -1)
                map.RotateFlip(RotateFlipType.Rotate180FlipNone);
            if (direction[1] == 1)
                map.RotateFlip(RotateFlipType.Rotate90FlipNone);
            if (direction[1] == -1)
                map.RotateFlip(RotateFlipType.Rotate270FlipNone);

            g.DrawImage(map, pt);
        }

        public static Bitmap GetParticipantBitmap(TeamColors teamColor, int[] direction)
        {
            Bitmap map = null;

            switch (teamColor)
            {
                case TeamColors.Blue:
                {
                    map = ImageLoader.GetBitmap(_carBlue);
                    break;
                }
                case TeamColors.Green:
                {
                    map = ImageLoader.GetBitmap(_carGreen);
                    break;
                }
                case TeamColors.Grey:
                {
                    map = ImageLoader.GetBitmap(_carGrey);
                    break;
                }
                case TeamColors.Red:
                {
                    map = ImageLoader.GetBitmap(_carRed);
                    break;
                }
                case TeamColors.Yellow:
                {
                    map = ImageLoader.GetBitmap(_carYellow);
                    break;
                }
            }

            if (map != null)
            {
                if (direction[0] == -1)
                    map.RotateFlip(RotateFlipType.Rotate180FlipNone);
                if (direction[1] == 1)
                    map.RotateFlip(RotateFlipType.Rotate90FlipNone);
                if (direction[1] == -1)
                    map.RotateFlip(RotateFlipType.Rotate270FlipNone);
            }

            return map;
        }

        public static Point GetParticipantCoords(Section section)
        {
            int x = section.X * visualWidth;
            int y = section.Y * visualHeight;

            return new Point(x,y);
        }

        // [0] offset, [1] direction
        public static int[][] GetSectionOffsets(SectionTypes type, int[] direction, int distance, bool left)
        {
            switch (type)
            {
                case SectionTypes.Finish: 
                case SectionTypes.Straight:
                case SectionTypes.StartGrid:
                {
                    // border: 25
                    if (direction[0] != 0)
                    {
                        distance = direction[0] > 0 ? distance : visualWidth - distance;
                        return new int[][] { new [] {distance, left ? 25 : 56}, direction};
                    }
                    else
                    {
                        distance = direction[1] > 0 ? distance : visualHeight - distance;
                        return new int[][] { new [] {left ? 25 : 56, distance}, direction};
                    }
                }
                case SectionTypes.LeftCorner:
                {
                    // border: 25, 56
                    distance = left ? distance : (distance / 2);
                    if (distance >= 50) // rotate player in turn
                        direction = Data.CurrentRace.GetDirection(direction, SectionTypes.LeftCorner);

                    if (direction[0] != 0)
                    {
                        distance = direction[0] > 0 ? distance : visualWidth - distance;
                        return new int[][] { new [] {distance, left ? 25 : 56}, direction};
                    }
                    else
                    {
                        distance = direction[1] > 0 ? distance : visualHeight - distance;
                        return new int[][] {new int[] {left ? 25 : 56, distance}, direction};
                    }
                }
                case SectionTypes.RightCorner:
                {
                    // border: 25, 56
                    distance = left ? distance : (distance / 2);
                    if (distance >= 50) // rotate player in turn
                        direction = Data.CurrentRace.GetDirection(direction, SectionTypes.RightCorner);

                    if (direction[0] != 0)
                    {
                        distance = direction[0] > 0 ? distance : visualWidth - distance;
                        return new int[][] {new int[] {distance, left ? 25 : 56}, direction};
                    }
                    else
                    {
                        distance = direction[1] > 0 ? distance : visualHeight - distance;
                        return new int[][] { new [] {left ? 25 : 56, distance}, direction};
                    }
                }
            }
            return new int[][] {new [] {0,0}, new [] {0,0}};
        }

        public static int[] GetTrackWidthHeight(Track track)
        {
            int minX = track.Sections.Min(x => x.X);
            int maxX = track.Sections.Max(x => x.X);
            int minY = track.Sections.Min(x => x.Y);
            int maxY = track.Sections.Max(x => x.Y);

            return new[] {(maxX - minX) + 1, (maxY - minY) + 1};
        }

        public static string GetSectionPath(SectionTypes type)
        {
            switch (type)
            {
                case SectionTypes.Finish:
                    return _finish;
                case SectionTypes.LeftCorner:
                    return _corner;
                case SectionTypes.RightCorner:
                    return _corner;
                case SectionTypes.StartGrid:
                    return _start;
                case SectionTypes.Straight:
                    return _straight;
            }
            return null;
        }

        public static Image GetSectionVisual(int[] direction, SectionTypes type)
        {
            string path = GetSectionPath(type);
            Bitmap map = ImageLoader.GetBitmap(path);
            map.Clone();

            switch (type)
            {
                case SectionTypes.Finish:
                    {
                        if(direction[0] != 0)
                        {
                            if (direction[0] > 0)
                                map.RotateFlip(RotateFlipType.RotateNoneFlipX);
                            return map;
                        }
                        else
                        {
                            if (direction[1] > 0)
                                map.RotateFlip(RotateFlipType.RotateNoneFlipY);
                            return map;
                        }
                    }
                case SectionTypes.LeftCorner:
                    {
                        map.RotateFlip(RotateFlipType.RotateNoneFlipY);

                        if (direction[0] == -1)
                            map.RotateFlip(RotateFlipType.Rotate180FlipNone);

                        if (direction[1] == -1)
                            map.RotateFlip(RotateFlipType.Rotate270FlipNone);

                        if (direction[1] == 1)
                            map.RotateFlip(RotateFlipType.Rotate90FlipNone);

                        return map;
                    }
                case SectionTypes.RightCorner:
                    {
                        if (direction[0] == -1)
                            map.RotateFlip(RotateFlipType.Rotate180FlipNone);

                        if (direction[1] == -1)
                            map.RotateFlip(RotateFlipType.Rotate270FlipNone);

                        if (direction[1] == 1)
                            map.RotateFlip(RotateFlipType.Rotate90FlipNone);

                        return map;
                    }
                case SectionTypes.StartGrid:
                    {
                        if (direction[0] != 0)
                        {
                            if (direction[0] < 0)
                                map.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        }
                        else
                        {
                            map.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            if (direction[1] == -1)
                                map.RotateFlip(RotateFlipType.RotateNoneFlipY);
                        }

                        return map;
                    }
                case SectionTypes.Straight:
                    {
                        if (direction[0] == 0)
                        {
                                map.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        }

                        return map;
                    }
            }
            return null;
        }
    }
}
