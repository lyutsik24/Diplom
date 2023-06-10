using System;
using System.Drawing;
using System.Windows.Forms;

public class FormDraggable
{
    private bool isDragging = false;
    private Point dragStartPosition;

    public void Attach(Control control)
    {
        control.MouseDown += Control_MouseDown;
        control.MouseMove += Control_MouseMove;
        control.MouseUp += Control_MouseUp;
    }

    private void Control_MouseDown(object sender, MouseEventArgs e)
    {
        isDragging = true;
        dragStartPosition = new Point(e.X, e.Y);
    }

    private void Control_MouseMove(object sender, MouseEventArgs e)
    {
        if (isDragging)
        {
            Control control = (Control)sender;
            Form form = control.FindForm();

            Point currentPosition = form.PointToScreen(e.Location);
            form.Location = new Point(currentPosition.X - dragStartPosition.X, currentPosition.Y - dragStartPosition.Y);
        }
    }

    private void Control_MouseUp(object sender, MouseEventArgs e)
    {
        isDragging = false;
    }
}
