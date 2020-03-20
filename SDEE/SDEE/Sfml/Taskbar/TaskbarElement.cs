using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE.Sfml
{
    //abstract class TaskbarElement : Control
    //{
    //    public MyTaskbar Taskbar => Parent as MyTaskbar;
    //    //internal int TaskbarPosition { get; set; }
    //    //public virtual int Width { get; set; }
    //    public override Vector2i Position
    //    public override Vector2i Size => new Vector2i(Width, Taskbar.Size.Y);

    //    //protected override void Init()
    //    //{
    //    //    //tbPos = GetFreeTaskbarPos();

    //    //    //base.Init();

    //    //    //int GetFreeTaskbarPos()
    //    //    //{
    //    //    //    int freeXPos = 0;
    //    //    //    int move = 0;

    //    //    //    do
    //    //    //    {
    //    //    //        move = (from control in Taskbar.Children.GetEachFiltered<TaskbarElement>()
    //    //    //                orderby control.Position.X ascending
    //    //    //                where control.Position.X >= freeXPos
    //    //    //                && control.Position.X + control.Size.X <= freeXPos + Size.X
    //    //    //                && control != this // Can stay if it's alone
    //    //    //                select control.Size.X)
    //    //    //                .FirstOrDefault();

    //    //    //        freeXPos += move;
    //    //    //    }
    //    //    //    while (move != 0);

    //    //    //    return freeXPos;
    //    //    //}
    //    //}
    //}
}
