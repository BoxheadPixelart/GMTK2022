using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using NaughtyAttributes;
using UnityEngine;

namespace Storm.Utils
{
   
    public class Numeric_Springing
    {
        public class FloatSpring
        {
             public float value;
             public float velo; 
        } 
        [System.Serializable]
        public class Vector3Spring
        {
            public Vector3 value;
            public Vector3 velo;
            [OnValueChanged("SetCurve")]
            public float damp;
            public float speed;
            public float freq;
            [OnValueChanged("SetCurve")]
            public AnimationCurve curve;

            public void SetCurve()
            {
                Debug.LogError("CURVE");
                curve = new AnimationCurve();
                float t  = 0;
                float tSpring = -0.1f;
                float tSpringv = -0.1f;

                while (Mathf.Abs(tSpring - 10) > 0 && Mathf.Abs(tSpringv) > 0 || t > 100)
                {
                    t += 0.1f; 
                    float[] result = Spring_Float(tSpring, tSpringv, 10, damp, freq, speed);
                    tSpring = result[0] + result[1]; 
                    Debug.LogError("Time vs Position: "  + t + " vs " + result[0]);
                    if ( t % 1 == 0)
                    {
                        curve.AddKey(t, tSpring);
                    }

                }
            }
            
            public Vector3 output
            {
                get
                {
                    return value + velo; 
                }
            }

            public Vector3Spring(float d, float f, float s)
            {
                value = new Vector3();
                velo = new Vector3(); 
                //
                damp = d;
                freq = f;
                speed = s; 


                //
            }
            public Vector3Spring Spring(Vector3 goalVector)
            {
                Vector3[] result = Spring_Vector3(value, velo, goalVector, damp, freq, speed);
                value = result[0];
                velo = result[1];
                return this;
            }
        }
       
        public static float[] Spring_Float(float value, float velocity, float target_value, float damping_ratio, float angular_frequency, float time_step)
        {
            //Math from: http://allenchou.net/2015/04/game-math-more-on-numeric-springing/ and http://allenchou.net/2015/04/game-math-precise-control-over-numeric-springing/
            float[] _ret = { 0, 0 };
            float _x = value;                          //Input value
            float _v = velocity * (120 / time_step);  //Input velocity
            float _x_t = target_value;               //Target value
            float _damping = damping_ratio;         //Damping of the oscillation (0 = no damping, 1 = critically damped)
            float _ang_freq = 2 * Mathf.PI * angular_frequency;             //Oscillations per second
            float _t = time_step / 120;              //How much of a second each step/use of the script takes (1 = normal time, 2 = twice as fast,..)
            float _delta_v, _delta_x, _delta;

            _delta = (1 + 2 * _t * _damping * _ang_freq) + Mathf.Pow(_t, 2) * Mathf.Pow(_ang_freq, 2);
            _delta_x = (1 + 2 * _t * _damping * _ang_freq) * _x + _t * _v + Mathf.Pow(_t, 2) * Mathf.Pow(_ang_freq, 2) * _x_t;
            _delta_v = _v + _t * Mathf.Pow(_ang_freq, 2) * (_x_t - _x);


            _ret[0] = _delta_x / _delta;                        //Output value
            _ret[1] = (_delta_v / _delta) / (120 / time_step); //Output velocity
            return _ret;
        }
        //
        public static float Spring_Float(float value, ref float velocity, float target_value, float damping_ratio, float angular_frequency, float time_step)
        {
            //Math from: http://allenchou.net/2015/04/game-math-more-on-numeric-springing/ and http://allenchou.net/2015/04/game-math-precise-control-over-numeric-springing/
            float _x = value;                        //Input value
            float _v = velocity * (120 / time_step);  //Input velocity
            float _x_t = target_value;                       //Target value
            float _damping = damping_ratio;                  //Damping of the oscillation (0 = no damping, 1 = critically damped)
            float _ang_freq = 2 * Mathf.PI * angular_frequency;             //Oscillations per second
            float _t = time_step / 120;              //How much of a second each step/use of the script takes (1 = normal time, 2 = twice as fast,..)
            float _delta_v, _delta_x, _delta;

            _delta = (1 + 2 * _t * _damping * _ang_freq) + Mathf.Pow(_t, 2) * Mathf.Pow(_ang_freq, 2);
            _delta_x = (1 + 2 * _t * _damping * _ang_freq) * _x + _t * _v + Mathf.Pow(_t, 2) * Mathf.Pow(_ang_freq, 2) * _x_t;
            _delta_v = _v + _t * Mathf.Pow(_ang_freq, 2) * (_x_t - _x);


            var ret = _delta_x / _delta;                        //Output value
            velocity = (_delta_v / _delta) / (120 / time_step); //Output velocity
            return ret;
        }
        ///
        //```
        public static Vector3[] Spring_Vector3(Vector3 value, Vector3 velocity, Vector3 target_value, float damping_ratio, float angular_frequency, float time_step)
        {
            //Math from: http://allenchou.net/2015/04/game-math-more-on-numeric-springing/ and http://allenchou.net/2015/04/game-math-precise-control-over-numeric-springing/
            Vector3[] _ret = { new Vector3(0, 0, 0), new Vector3(0, 0, 0) };

            // X Spring
            float[] x = Spring_Float(value.x, velocity.x, target_value.x, damping_ratio, angular_frequency, time_step);
            //
            float[] y = Spring_Float(value.y, velocity.y, target_value.y, damping_ratio, angular_frequency, time_step);
            //
            float[] z = Spring_Float(value.z, velocity.z, target_value.z, damping_ratio, angular_frequency, time_step);
            //

            _ret[0] = new Vector3(x[0], y[0], z[0]);
            _ret[1] = new Vector3(x[1], y[1], z[1]);
            return _ret;
        }
        public static Vector3 Spring_Vector3(Vector3 value, Vector3 target_value, float damping_ratio, float angular_frequency, float time_step)
        {
            Vector3  velocity = new Vector3(); 
            //Math from: http://allenchou.net/2015/04/game-math-more-on-numeric-springing/ and http://allenchou.net/2015/04/game-math-precise-control-over-numeric-springing/
            Vector3 _ret = new Vector3(0, 0, 0); 

            // X Spring
        
            float[] x = Spring_Float(value.x, velocity.x, target_value.x, damping_ratio, angular_frequency, time_step);
            //
            float[] y = Spring_Float(value.y, velocity.y, target_value.y, damping_ratio, angular_frequency, time_step);
            //
            float[] z = Spring_Float(value.z, velocity.z, target_value.z, damping_ratio, angular_frequency, time_step);
            //

            _ret = new Vector3(x[0], y[0], z[0]);
            velocity = new Vector3(x[1], y[1], z[1]);
            return _ret;
        }
        public static Vector3[] Spring_Rotation(Vector3 value, Vector3 velocity, Vector3 target_value, float damping_ratio, float angular_frequency, float time_step)
        {
            float xRotDiff;
            float yRotDiff;
            float zRotDiff;
            Vector3[] _ret = { new Vector3(0, 0, 0), new Vector3(0, 0, 0) };
            //
            xRotDiff = value.x - Mathf.DeltaAngle(target_value.x, value.x);
            float[] xrot = Spring_Float(value.x, velocity.x, xRotDiff, damping_ratio, angular_frequency, time_step);
            //
            yRotDiff = value.y - Mathf.DeltaAngle(target_value.y, value.y);
            float[] yrot = Spring_Float(value.y, velocity.y, yRotDiff, damping_ratio, angular_frequency, time_step);
            //
            zRotDiff = value.z - Mathf.DeltaAngle(target_value.z, value.z);
            float[] zrot = Spring_Float(value.z, velocity.z, zRotDiff, damping_ratio, angular_frequency, time_step);
            //
            _ret[0] = new Vector3(xrot[0], yrot[0], zrot[0]);
            _ret[1] = new Vector3(xrot[1], yrot[1], zrot[1]);
            //
            return _ret;
        }
        //  public static float[] Spring_RotationAngle(Vector3 value, Vector3 velocity, Vector3 target_value, float damping_ratio, float angular_frequency, float time_step)
        //  {
        //  float RotDiff;
        // float[] _ret = {0,0}; 
        // return _ret;
        // }
        // public static Transform[] Spring_Transform(Vector3 value, Vector3 velocity, Vector3 target_value, float damping_ratio, float angular_frequency, float time_step)
        // {
        //Transform
        //return _ret; 
        //  }
        //

        //

        //

    }
}
