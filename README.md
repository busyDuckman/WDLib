# Overview

WDLib is the Personal “Toolbox” of Dr Warren Creemers. The Code base dates back to the first beta realises of .net in the later part 2000. 
As the code base is embedded in several applications and assists with rapid software development.

# Notes:
  - Some parts of  WDLib were made redundant as new features were added to .NET; they are kept in place to allow legacy products to compile.
  - Maintaining “backward compatibility” in the library is otherwise NOT a priority.
  - __Some areas of code are 20 years old__ and may be in need of improvement.
  - An effort to provide a good suite of unit tests is underway.
  - Generally amount of documentation a class has, is a good indication of how well used and tested it was.
  - A few classes are viewed as “OK” to be tightly coupled into the rest of the library.
    - Eg Logging, .NET extension methods, the Why class and so on.
  - Periodically all compiler warnings are attended to.


# Manifesto:
  - WDLib is intended to provide “catch all” helper classes. If a problem does not have a solution that works for all/most situations, then the problem is not addressed in WDLib.
  - WDLibToolBox and WDAplicationFramework should never use a 3rd party library.
  - 3rd party libraries should otherwise be kept to an essential minimum, and loosely coupled.
  - Any application should be able to cherry pick put and use only the parts of WDLib they want. 
  - No part of WDlib should ever force the application developer to incorporate extra classes due to poor coupling practices.
  - XML Documentation of all classes is a priority.
  - Code should be “interface driven” wherever possible.
  - Interfaces should use extension methods to implement anything that is in essence just logic around the interface, or a variant of a method call.

All WDlib data structures should meet the following checklist:
  - Sensible and efficient hash method
  - Implementation of IClonable
    - via a call to a protected copy constructor.
  - Implementation of IEquatable
  - Public members are CapitalCase (TODO: this is a change in convention for this library, lots of older code does not do this.)

The following attributes should be used whenever applicable:
  - [NonSerialized] 
  - [Serializable] 




# Projects

## WDLib
A generic toolbox.

## WDLibApplicationFramework
A toolbox for Winforms Development.


## WDDevice
Concerened with talking to external devices and communication with hardware.

## Test_WDLib
Unit tests for the library

## WDMedia
A toolbox for simple game development.

# FindUsage
Used to help track references to WDLib in my code.

# Key Classes

## IRenderer
All graphics work is generally done through a wrapper (IRenderer). The idea is that:
  - Software using this wrapper is graphics API agnostic
  - By alternating the IRenderer to a software output HPGL / DXF can be created from the same logic that does screen output.
  - Wrapper is based on .NET GDIplus to allow for easy porting.

Concepts
  - Supports reduced colour palate’s via  GetNearestColor(...)
  - If the API supports being altered by threads other than the one that created the context SupportsMultiThreading returns true.
  - Angles are always double precision and are always in the units given by AngleType. AngleType is always able to be altered to suit.
  - Initial value is undefined.
  - Line Endcaps are can be either Flat or Round.
  - Quality is either High or low. see SetHighQuality(..).
  - Low quality should not cause any interpolation of pixels when bitmaps are rendered.
  - Low quality will disable all antialiasing.
  - SetHighQuality(..) can be called at any time, and results aplicable only to items rendered after the setting is changed.
  - High quality sets all features of a rendering API to their highest setting.
  - Any newly created renderer should be in high quality mode by default.
  - The renderer is expected to handle clipping gracefully
  - RenderTargetAsGDIBitmap() must be implemented. Is causes all output usable as a Bitmap.
  - IDisposable is implemented, and should ONLY call the close method.
  - Close() will flush all output and free cached resourcves..
  - Close() may finalise the rendering context of the API. The IRenderer may prevent this for whatever reason (normally when the graphics context relates to a screen, not a file or software device).
  - Close() can be called multiple times without issue.
  - The renderer is free to add additional output settings.
  - All settings are stored in a class extending IIRendererSettings and retrived / applied via GetSettings() and ApplySettings(...)
  - It is expected that IIRendererSettings can be used in real-time via a stack
  - Extension method GetContext() returns a RenderingContext which is a wrapped instance of the renderer that restores settings changes when it is disposed.


__Example:__

        private void picMain_Paint(object sender, PaintEventArgs e)
        {
            if (weldImageView != null)
            {
                GDIPlusRenderer r = new GDIPlusRenderer(e.Graphics);
                using (var rs = r.GetContext())
                {
                    rs.Transform = TransMatrix2D.FromScale(0.5);
                    rs.DrawLine(Color.Red, 2, 0, 0, 100, 100);
                }
	      	   r.Close(); //has no effect in this case.
            }
        }

IRenderable is not finished yet.


## View System
The view system refers to infrastructure designed to provide for a 2d view window (pan/zoom/rotate). The concept of a view is encapsulated in IView2D.  Infrastructure such as View2DBase and related classes make implementing a IView2D easy. IView2D renders to any IRenderer, so how the view is displayed is very flexible.  A control, ViewBox2D, is provided as a quick means to turn a view into a GUI element.
 
The power of the infrastructure is in how quickly a view system can be created. Eg. for an image:
  - Drop a ViewBox2D on a form
  - In the code editor set its View property to a new RasterView2D
  - Assign your bitmap to the RasterView2D’s Image property
The view system generally deals in world space co-ordinates. However there are times when “Widgets” or “Gizmos” must be drawn on the screen. The IEditableView2D allows for Gizmos to be drawn. The class provides a second rendering method RenderGizmoLayer which is called after the regular Render method.

 

Editable views are presented all mouse events in world space, and must implement their own controller code to handle this. A set of classes based around IGizmo provide a typical pattern, for vector editors, to help with such operations.  However they are optional and the implementation of the gizmo/widget controller is left to the application developer.
 
It’s important for views to have and on screen grid option.
Grids should be:
  - Extensible
  - Able to “snap” mouse input.
  - Show accurate rulers / measurements
  - Rendered in the gizmo layer (screen – space)
