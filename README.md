# DouBOL-Dash
An upcoming editor for Mario Kart: Double Dash!! track files, and possibly more!

# How to use
You basically need to have Visual Studio installed. I built this one with VS2015, but I don't think this will make much of a difference.

Or you can just run the EXE found in /bin. Currently if you save your work, you have to manually insert the 0x1A bytes back into the beginning of the BOL file.

Ever since 7/27/2016, you can now save just the objects section, and parts of the header. You manually take out the objects and put them into the original file to apply changes. Each object entry is 0x40 bytes.

Ever since 7/30/2016, said saving has been disabled due to a code structure change. This will cancel out saving until everything else can be saved into a new file.

# What's supported
Viewing of every single section of the file, except for a useless section, and a bit of the header.

# Special Thanks
I'd like to thank Chadderz for the EndianBinaryReader library found in System/IO, which allows for Big Endian Binary reading. :)

I'd also like to thank Marionumber1 for helping me out a bit with the rotation calculations.

And I'd like to thank the people that made the icons.