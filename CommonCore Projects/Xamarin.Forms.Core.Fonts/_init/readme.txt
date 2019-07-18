

Step 1:
    Copy files from the FontFiles folder in this project to:
    Droid:
        Assets folder add the files as android assets.
    iOS:
        Resource folder -> create Sub folder called Fonts and add the files as bundle resources.

Step 2:

    iOS:
        In the info.plist add under the source tab a category called 'Fonts provided by application' (Array type)
        Add to the array a string entry for each font file (i.e. Fonts/fontawesome.ttf)



Example use:

    Create a style:
        public static Style FAHome { get; } = new Style(typeof(Label))
        {
            Setters =
            {
                new Setter(){Property=Label.FontSizeProperty ,Value=55},
                new Setter(){Property=Label.MarginProperty ,Value=10},
                new Setter(){Property=Label.HorizontalOptionsProperty ,Value=LayoutOptions.Center},
                new Setter(){Property=Label.HorizontalTextAlignmentProperty ,Value=TextAlignment.Center},
                new Setter(){Property=Label.FontFamilyProperty ,Value=FontType.FontAwesome.ToString()},
                new Setter(){Property = Label.TextProperty, Value=FontUtil.GetFont("fa-home",FontType.FontAwesome).Unicode}
            }
        };

   Create a label
        var iconLabel = new Label()
        {
            Style = CoreStyles.FAHome,
            TextColor = Color.Blue
        };