using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space
{
    public class State
    {

        /*
        We're going to have an array of objects, each of these objects will have its position.
        There will be an objective centre of the universe but we're not going to store every point, obvoiusly.
        We can math our way through to determine what is visible and what is not.
        So we're going to have a general object that has a position relative to the origin.
        Of which can be a model or it can be an observer i suppose.
        One of these objects needs:
        - an array of points (3-d points) 
        - a position
        */
        List<Figure3D> figures { get; set; }
        
        public State()
        {
            figures = new List<Figure3D>();
        }
        public void addFigure(Figure3D fig)
        {
            figures.Add(fig);
        }
        public void UpdateState()
        {
            
        }
    }
}
