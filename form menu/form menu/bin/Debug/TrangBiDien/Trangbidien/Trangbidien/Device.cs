using MindFusion.Diagramming;
using System.Drawing;
using System.Collections.Generic;


namespace Trangbidien
{
    public class Device
    {

        public ShapeNode shapenode;
        public PointF prePos;
        public int PortCount;
        public List<PointF> port;
        public float width, height;
        public Device(int count)
        {
            PortCount = count;
            port = new List<PointF>();
        }

        /// <summary>
        /// Set ShapeNode of the Device
        /// </summary>
        /// <param name="node"></param>
        public void SetNode(ShapeNode node)
        {
            this.shapenode = node;
            prePos = new PointF(node.Bounds.X, node.Bounds.Y);
            this.width = node.Bounds.Width;
            this.height = node.Bounds.Height;
        }

        public void SetPrePos(PointF pre)
        {
            this.prePos = pre;
        }

        public void UpdatePortPosition(PointF prePosition, PointF currentPosition)
        {
            for (int i = 0; i < port.Count; i++)
            {
                float x = port[i].X + currentPosition.X - prePosition.X;
                float y = port[i].Y + currentPosition.Y - prePosition.Y;

                port[i] = new PointF(x, y);

            }

        }


    }
}
