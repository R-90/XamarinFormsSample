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
            Vector3 pButtonScale = new Vector3(1.5f, 0.5f, 0.25f);
            Vector3 stickScale = new Vector3(1, 1, 0.5f);
            scene = new Scene();
            octree = scene.CreateComponent<Octree>();

            mainNode = scene.CreateChild();
            mainNode.Position = new Vector3(0, 0, 10);
            Node stickNode = mainNode.CreateChild(name: "stick");
            Sphere stick = stickNode.CreateComponent<Sphere>();
            stick.Color = Color.White;
            
            stickNode.Scale = stickScale;

            Node pUpNode = mainNode.CreateChild(name: "panUp");
            Node pDownNode = mainNode.CreateChild(name: "panDown");
            Node pRightNode = mainNode.CreateChild(name: "panRight");
            Node pLeftNode = mainNode.CreateChild(name: "panLeft");
            Node backgroundNode = mainNode.CreateChild(name: "background");

            Pyramid pUp = CreatePyramid(pUpNode);
            Pyramid pDown = CreatePyramid(pDownNode);
            Pyramid pRight = CreatePyramid(pRightNode);
            Pyramid pLeft = CreatePyramid(pLeftNode);

            pUpNode.Scale = pButtonScale;
            pDownNode.Scale = pButtonScale;
            pRightNode.Scale = pButtonScale;
            pLeftNode.Scale = pButtonScale;

            pUpNode.Position = new Vector3(0, 3.75f, 0);
            pDownNode.Position = new Vector3(0, -3.75f, 0);
            pRightNode.Position = new Vector3(3f, 0, 0);
            pLeftNode.Position = new Vector3(-3f, 0, 0);

            pRightNode.Rotation = new Quaternion( 0,  0, -90);
            pLeftNode.Rotation = new Quaternion( 0, 0, 90);
            pDownNode.Rotation = new Quaternion(0, 0, 180);

            var cameraNode = scene.CreateChild("camera");
            camera = cameraNode.CreateComponent<Camera>();
            
            Node lightNode = cameraNode.CreateChild(name: "light");
            var light = lightNode.CreateComponent<Light>();
            light.LightType = LightType.Directional;
            lightNode.SetDirection(new Vector3(0,0,1));
        }

        

        void SetupViewport()
        {
            var renderer = Renderer;
            renderer.SetViewport(0, new Viewport(Context, scene, camera, null));
        }
    }

    // moving stick component to it's own class.
    public class Stick : Component
    {
        Node stickNode;
        Color color;
        float lastUpdateValue;


        public Vector3 Value
        {
            get { return stickNode.Position; }
            set { stickNode.Position = new Vector3( value.X, value.Y, 0 ); }
        }

        public Stick(Color color)
        {
            this.color = color;
        }

        public override void OnAttachedToNode(Node node)
        {
            stickNode = node.CreateChild();
            stickNode.Scale = new Vector3(1, 1, 0.5f); 
            Sphere sphere = stickNode.CreateComponent<Sphere>();
            sphere.Color = color;

            base.OnAttachedToNode(node);
        }


        protected override void OnUpdate(float timeStep)
        {
            var input = Application.Current.Input;
            if (input.NumTouches > 0)
            {
                TouchState state = input.GetTouch(0);
                var touchPosition = state.Position;
                //stickPosX = touchPosition.X;
                //stickPos
            }

            base.OnUpdate(timeStep);
            var stickPos = stickNode.Position;

            stickNode.Position = new Vector3(stickPos.X, stickPos.Y, 0);
        }

        
    }
}
