using System;
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
                    GridItem item;
                    if (counter == 15)
                    {
                        item = new GridItem(new GridPosition(row, col), "empty");
                        item.FinalLabel = "16";
                    }
                    else
                    {
                        item = new GridItem(new GridPosition(row, col), counter.ToString());
                    }

                    var tapRocognizer = new TapGestureRecognizer();
                    tapRocognizer.Tapped += OnLabelTapped;
                    item.GestureRecognizers.Add(tapRocognizer);

                    _gridItems.Add(item.CurrentPosition, item);
                    _absoluteLayout.Children.Add(item);

                    counter++;

                }
            }

            Shuffle();
            Shuffle();
            Shuffle();
            Shuffle();

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

            //Did we click on empty? If so do nothing
            if (item.isEmptySpot() == true)
            {
                return;
            }
            // We know we didn't click on the empty spot

            // Check up, down, left, right, until we find empty
            GridPosition pos = null;
            var counter = 0;
            while (counter < 4)
            {
                if (counter == 0 && item.CurrentPosition.Row != 0)
                {
                    //Get position of square above current item
                    pos = new GridPosition(item.CurrentPosition.Row - 1, item.CurrentPosition.Column);
                }
                else if (counter == 1 && item.CurrentPosition.Column != SIZE - 1)
                {
                    //Get position of square to right of current item
                    pos = new GridPosition(item.CurrentPosition.Row, item.CurrentPosition.Column + 1);
                }
                else if (counter == 2 && item.CurrentPosition.Row != SIZE - 1)
                {
                    //Get postion of square below the item
                    pos = new GridPosition(item.CurrentPosition.Row + 1, item.CurrentPosition.Column);
                }
                else if (counter == 3 && item.CurrentPosition.Column != 0)
                {
                    // Get position of square to left of current item
                    pos = new GridPosition(item.CurrentPosition.Row, item.CurrentPosition.Column - 1);
                }


                int row = 0;
                int col = 0;

                if (pos != null)
                {
                    GridItem swapWith = _gridItems[pos];
                    if (swapWith.isEmptySpot())
                    {
                        Swap(item, swapWith);
                        break;
                    }

                }
                counter++;


            }

            OnContentViewSizeChanged(this.Content, null);
        }

        void Shuffle()
            {
                Random rand = new Random();
                for (var row=0; row < SIZE ; row++)
                {
                    for (var col=0; col<SIZE; col++)
                    {
                    GridItem item = _gridItems[new GridPosition(row, col)];

                    int swapRow = rand.Next(0, 4);
                    int swapCol = rand.Next(0, 4);
                    GridItem swapItem = _gridItems[new GridPosition(swapRow, swapCol)];

                    Swap(item, swapItem);

                    }
                }
            }

            void Swap(GridItem item1, GridItem item2)
            {
                //First Swap Positions
                GridPosition temp = item1.CurrentPosition;
                item1.CurrentPosition = item2.CurrentPosition;
                item2.CurrentPosition = temp;

            //Then update the dictionary too!
            _gridItems[item1.CurrentPosition] = item1;
            _gridItems[item2.CurrentPosition] = item2;

            }

         }

    //class GridItem : Label
    //{
    //    public GridPosition CurrentPosition
    //    {
    //        get; set;
    //    }

    //    public GridItem(GridPosition position, String text)
    //    {
    //        CurrentPosition = position;
    //        Text = text;
    //        TextColor = Color.White;
    //        //img.Source = FileImageSource.FromFile("icon.png");
    //        HorizontalOptions = LayoutOptions.Center;
    //        VerticalOptions = LayoutOptions.Center;
    //    }
    //}

    class GridItem : Image
    {
        public GridPosition CurrentPosition
        {
            get; set;
        }

        private GridPosition _finalPosition;
        private Boolean _isEmptySpot;
        private String Text;

        public String FinalLabel
        {
            get; set;
        }


        public GridItem(GridPosition position, String text)
        {
            _finalPosition = position;
            CurrentPosition = position;
            Text = text;

             if (text.Equals("empty"))
            {
                _isEmptySpot = true;
            }
             else
            {
                _isEmptySpot = false;
            }
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

            if (text != "empty")
            {
                int imagenumber = Convert.ToInt32(text.ToString());
                Source = ImageSource.FromResource(imagelist[imagenumber]);

                HorizontalOptions = LayoutOptions.CenterAndExpand;
                VerticalOptions = LayoutOptions.CenterAndExpand;

            }
        }

        public Boolean isEmptySpot()
        {
            return _isEmptySpot;
        }

        public void showFinalLabel()
        {
            if (isEmptySpot())
            {
               Text = this.FinalLabel;
            }
        }

        public Boolean isPositionCorrect()
        {
            return _finalPosition.Equals(CurrentPosition);
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
