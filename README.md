
# C# implement of 'Efficient Graph-Based Image Segmentation' 


### Article
---
Felzenszwalb, P.F., Huttenlocher, D.P. Efficient Graph-Based Image Segmentation. International Journal of Computer Vision 59, 167–181 (2004). 
https://doi.org/10.1023/B:VISI.0000022288.19776.77


<p align="center">
    <img src="https://github.com/jaewoo-so/Cs_Graph-Based_Image_Segmentation/blob/master/img/paper_example1.jpg?raw=true"  width="400" />
    <br/>
    <img src="https://github.com/jaewoo-so/Cs_Graph-Based_Image_Segmentation/blob/master/img/paper_example2.jpg?raw=true"  width="400" />
</p>


        
### Simple Example 
---

```csharp
using Emgu.CV;
using Emgu.CV.Structure;
using GrpahBasedImageSegementation;
using SpeedyCoding;

testdata = new Image<Gray , byte>( ofd.FileName ).Data.ToJagged().Select( f => f.Select( s => s [ 0 ] ).ToArray() ).ToArray();

GBS gg = new GBS();
var result = gg.Processing(testdata , 80);
var data = result.Coloring().ToMat();
Image<Rgb,byte> output = new Image<Rgb, byte>(data);

output.Save( "SAVE PATH" );

```

**[original image]**    
<img src="https://github.com/jaewoo-so/Cs_Graph-Based_Image_Segmentation/blob/master/img/Test.bmp"  width="400" />


**[result image]**    
<img src="https://github.com/jaewoo-so/Cs_Graph-Based_Image_Segmentation/blob/master/img/Result.bmp"  width="400" />
    


    
# Dependency
- EmguCV = 3.3.0
- SpeedyCoding = 2.2.1


** SpeedyCoding is my personal libaray for funtional programing & chaining style coding for C#. 