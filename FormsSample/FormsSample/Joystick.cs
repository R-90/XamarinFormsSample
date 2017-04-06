using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho.Forms;
using Urho;
using Urho.Shapes;

namespace FormsSample
{
    public class Joystick : Application
    {
        bool movementEnabled;
        Scene scene;
        Camera camera;
        Node mainNode;
        Octree octree;


        [Preserve]
        public Joystick(ApplicationOptions options = null) : base(options) { }

        static Joystick()
        {
            UnhandledException += (s, e) =>
            {
                if (Debugger.IsAttached)
                    Debugger.Break();
                e.Handled = true;
            };
        }

        protected override void Start()
        {
            base.Start();
            CreateScene();
            SetupViewport();
        }

        private Pyramid CreatePyramid(Node node)
        {
            Pyramid pyramid = node.CreateComponent<Pyramid>();
            pyramid.Color = Color.White;
            return pyramid;
        }

        async void CreateScene()
        {
            //Input.SubscribeToTouchEnd(OnTouched);

            var cache = ResourceCache;
            Vector3 pButtonScale = new Vector3(2, 1, 0.5f);
            scene = new Scene();
            octree = scene.CreateComponent<Octree>();

            mainNode = scene.CreateChild();
            mainNode.Position = new Vector3(0, 0, 10);
            Node stickNode = mainNode.CreateChild();
            Sphere stick = stickNode.CreateComponent<Sphere>();
            stick.Color = Color.White;

            Node pUpNode = mainNode.CreateChild(name: "panUp");
            Node pDownNode = mainNode.CreateChild(name: "panDown");
            Node pRightNode = mainNode.CreateChild(name: "panRight");
            Node pLeftNode = mainNode.CreateChild(name: "panLeft");

            Pyramid pUp = CreatePyramid(pUpNode);
            Pyramid pDown = CreatePyramid(pDownNode);
            Pyramid pRight = CreatePyramid(pRightNode);
            Pyramid pLeft = CreatePyramid(pLeftNode);

            pUpNode.Scale = pButtonScale;
            pDownNode.Scale = pButtonScale;
            pRightNode.Scale = pButtonScale;
            pLeftNode.Scale = pButtonScale;

            pUpNode.Position = new Vector3(0, 2, 0);
            pDownNode.Position = new Vector3(0, -2, 0);
            pRightNode.Position = new Vector3(2, 0, 0);
            pLeftNode.Position = new Vector3(-2, 0, 0);

            //pRightNode.Rotation = new Quaternion(Vector3.UnitZ, 90);
            //pLeftNode.Rotation = new Quaternion(Vector3.UnitZ, -90);
            //pDownNode.Rotation = new Quaternion(Vector3.UnitZ, 180);

            //var plane = baseNode.CreateComponent<StaticModel>();
            //plane.Model = ResourceCache.GetModel("Models/Plane.mdl");

            var cameraNode = scene.CreateChild("camera");
            camera = cameraNode.CreateComponent<Camera>();
            
            Node lightNode = cameraNode.CreateChild(name: "light");
            var light = lightNode.CreateComponent<Light>();
            light.LightType = LightType.Directional;
            lightNode.SetDirection(new Vector3(0,0,1));
            light.Range = 100;
            //light.Brightness = 1.3f;

            
        }

        void SetupViewport()
        {
            var renderer = Renderer;
            renderer.SetViewport(0, new Viewport(Context, scene, camera, null));
        }
    }
}
