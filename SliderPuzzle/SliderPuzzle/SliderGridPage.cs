﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace SliderPuzzle
{
    public class SliderGridPage : ContentPage
    {
        private const int SIZE = 4;

        private AbsoluteLayout _absoluteLayout;
        private Dictionary<GridPosition, GridItem> _gridItems;
        private Image picture_00;



        public SliderGridPage()
        {
            _gridItems = new Dictionary<GridPosition, GridItem>();
            _absoluteLayout = new AbsoluteLayout
            {
                BackgroundColor = Color.Blue,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            var counter = 0;
            // Create the array of images.
           // Image[] imageList = new Image[1]
            //{
            //    picture_00
            //};


            //imageList[1].Source = ImageSource.FromFile("icon.png");
            //picture_00.Source = ImageSource.FromFile("icon.png");

            for (var row = 0; row < 4; row++)
            {
                for (var col = 0; col < 4; col++)
                {
                    GridItem item = new GridItem(new GridPosition(row, col), counter.ToString());
                    var tapRocognizer = new TapGestureRecognizer();
                    tapRocognizer.Tapped += OnLabelTapped;
                    item.GestureRecognizers.Add(tapRocognizer);

                    _gridItems.Add(item.Position, item);
                    _absoluteLayout.Children.Add(item);

                    counter++;

                }
            }

            ContentView contentView = new ContentView
            {
                Content = _absoluteLayout
            };
            contentView.SizeChanged += OnContentViewSizeChanged;
            this.Padding = new Thickness(5, Device.OnPlatform(25, 5, 5), 5, 5);
            this.Content = contentView;
        }

            void OnContentViewSizeChanged(object sender, EventArgs args)
            {
                ContentView contentView = (ContentView)sender;
                double squareSize = Math.Min(contentView.Width, contentView.Height) / 4;
                
                for (var row = 0; row < 4; row++)
                {
                    for (var col = 0; col<4; col++)
                    {
                        GridItem item = _gridItems[new GridPosition(row, col)];
                        Rectangle rect = new Rectangle(col * squareSize, row * squareSize, squareSize, squareSize);
                        AbsoluteLayout.SetLayoutBounds(item, rect);
                    }
                }
            }

            void OnLabelTapped(object sender, EventArgs args)
            {
                GridItem item = (GridItem)sender;
                Random rand = new Random();
                int move = rand.Next(0, 4);


                //Adjust random move to account for edges
                if (move == 0 && item.Position.Row == 0)
                {
                    move = 2;
                }
                else if (move == 1 && item.Position.Column == SIZE - 1)
                {
                    move = 3;
                }
                else if (move == 2 && item.Position.Row == SIZE - 1)
                {
                    move = 0;
                }
                else if (move == 3 && item.Position.Column == 0)
                {
                    move = 1;
                }

                int row = 0;
                int col = 0;

                if (move == 0) //Move up
                {
                    row = item.Position.Row - 1;
                    col = item.Position.Column;
                }
                else if (move == 1) //Move Right
                {
                    row = item.Position.Row;
                    col = item.Position.Column + 1;
                }
                else if (move == 2) //Move Down
                {
                    row = item.Position.Row + 1;
                    col = item.Position.Column;
                }
                else if (move == 3) //Move Left
                {
                    row = item.Position.Row;
                    col = item.Position.Column - 1;
                }

                GridItem swapWith = _gridItems[new GridPosition(row, col)];
                Swap(item, swapWith);
                OnContentViewSizeChanged(this.Content, null);


            }

            void Swap(GridItem item1, GridItem item2)
            {
                //First Swap Positions
                GridPosition temp = item1.Position;
                item1.Position = item2.Position;
                item2.Position = temp;

            //Then update the dictionary too!
            _gridItems[item1.Position] = item1;
            _gridItems[item2.Position] = item2;

            }
         }

    //class GridItem : Label
    //{
    //    public GridPosition Position
    //    {
    //        get; set;
    //    }

    //    public GridItem(GridPosition position, String text)
    //    {
    //        Position = position;
    //        Text = text;
    //        TextColor = Color.White;
    //        //img.Source = FileImageSource.FromFile("icon.png");
    //        HorizontalOptions = LayoutOptions.Center;
    //        VerticalOptions = LayoutOptions.Center;
    //    }
    //}

    class GridItem : Image
    {
        public GridPosition Position
        {
            get; set;
        }

        public GridItem(GridPosition position, String text)
        {
            Position = position;
            String[] imagelist = new String[16];
            imagelist[0] = "SliderPuzzle.Images.image-0-0.jpeg";
            imagelist[1] = "SliderPuzzle.Images.image-0-1.jpeg";
            imagelist[2] = "SliderPuzzle.Images.image-0-2.jpeg";
            imagelist[3] = "SliderPuzzle.Images.image-0-3.jpeg";
            imagelist[4] = "SliderPuzzle.Images.image-1-0.jpeg";
            imagelist[5] = "SliderPuzzle.Images.image-1-1.jpeg";
            imagelist[6] = "SliderPuzzle.Images.image-1-2.jpeg";
            imagelist[7] = "SliderPuzzle.Images.image-1-3.jpeg";
            imagelist[8] = "SliderPuzzle.Images.image-2-0.jpeg";
            imagelist[9] = "SliderPuzzle.Images.image-2-1.jpeg";
            imagelist[10] = "SliderPuzzle.Images.image-2-2.jpeg";
            imagelist[11] = "SliderPuzzle.Images.image-2-3.jpeg";
            imagelist[12] = "SliderPuzzle.Images.image-3-0.jpeg";
            imagelist[13] = "SliderPuzzle.Images.image-3-1.jpeg";
            imagelist[14] = "SliderPuzzle.Images.image-3-2.jpeg";
            imagelist[15] = "SliderPuzzle.Images.image-3-3.jpeg";

            int imagenumber = Convert.ToInt32(text.ToString());
            Source = ImageSource.FromResource(imagelist[imagenumber]);
            HorizontalOptions = LayoutOptions.Center;
            VerticalOptions = LayoutOptions.Center;
        }
    }

    class GridPosition
    {
        public int Row
        {
            get; set;
        }

        public int Column
        {
            get; set;
        }

        public GridPosition(int row, int col)
        {
            Row = row;
            Column = col;
        }

        public override bool Equals(object obj)
        {
            GridPosition other = obj as GridPosition;

            if (other != null && Row == other.Row && Column == other.Column)
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return 17 * (23 + this.Row.GetHashCode()) * (23 + this.Column.GetHashCode());
        }

    } 
}
