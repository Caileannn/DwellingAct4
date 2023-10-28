using Minis;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

[ExecuteInEditMode]
[System.Serializable]
public class MidiLiveMap : MonoBehaviour
{
    public VisualEffect m_VFX;

    // All Controllable Variables for Act 4 PJ

    private int m_PLifeMin;
    private int m_PLifeMax;
    private int m_PRate;
    private int m_PNoise;
    private int m_PTurbPosition;
    private int m_PRatio;
    //private int m_PGravity;
    private int m_PFreq;
    private int m_PDrag;
    private int m_ALRate;
    private int m_PTurb;

    public float f_PLifeMin = 5f;
    public float f_PLifeMax = 20f;
    public float f_PRate = 70000f;
    public float f_PNoise = 0.5f;
    public float f_PTurbPosition = 30f;
    public float f_PRatio = 10f;
    //public float f_PGravity = 0f;
    public float f_PFreq = 2f;
    public float f_PTurb = 0.5f;
    public float f_PTurbDrag = 8f;
    public float f_ALRate = 1000f;

    
    



    private float m_Slider = 0f;

    private float m_Potent = 0f;



    private float[] m_SliderIn = new float[8];
    private float[] m_PotIn = new float[8];

    void Start()
    {
        m_PLifeMin = Shader.PropertyToID("Pont Lifetime Min");
        m_PLifeMax = Shader.PropertyToID("Point Lifetime Max");
        m_PRate = Shader.PropertyToID("Point Rate");

        m_PNoise = Shader.PropertyToID("Noise");

        m_PRatio = Shader.PropertyToID("Turbulences Ratio");
        m_PTurbPosition = Shader.PropertyToID("Turbulences Speed");
        m_PFreq = Shader.PropertyToID("Turbulences Frequency");
        m_PTurb = Shader.PropertyToID("Turbulences");
        m_PDrag = Shader.PropertyToID("Turbulence Drag");

        m_ALRate = Shader.PropertyToID("Antenna Rate");

        // m_PGravity = Shader.PropertyToID("Gravity Point");


        InputSystem.onDeviceChange += (device, change) =>
        {
            if (change != InputDeviceChange.Added)
            {

                return;
            }

            var midiDevice = device as Minis.MidiDevice;

            if (midiDevice == null)
            {

                return;
            }

            midiDevice.onWillControlChange += (device, change) =>
            {
                if (device.controlNumber < 8)
                {
                    m_SliderIn[device.controlNumber] = (float)change;
                }

                if (device.controlNumber >= 16 && device.controlNumber <= 23)
                {

                    m_PotIn[device.controlNumber - 16] = (float)change;
                }

                // Debug.Log(device.controlNumber);
                Debug.Log(m_Slider + ":" + m_Potent);

                if (device.controlNumber == 32)
                {
                    // f_PGravity = -9.8f * (float)change;
                }



            };
        };

    }

    void Update()
    {
        // Debug.Log(m_Slider);    

        SetupVFXTest();
    }

    void SetupVFXTest()
    {
        
            // Points 
            m_VFX.SetFloat(m_PLifeMin, f_PLifeMin * m_PotIn[0]);
            m_VFX.SetFloat(m_PLifeMax, f_PLifeMax * m_PotIn[1]);
            m_VFX.SetFloat(m_PRate, f_PRate * m_PotIn[2]);
            m_VFX.SetFloat(m_PNoise, f_PNoise * m_PotIn[3]);

            // Vel Lines
            m_VFX.SetFloat(m_ALRate, f_ALRate * m_PotIn[4]);

            // Turbulence's
            m_VFX.SetFloat(m_PRatio, f_PRatio * m_SliderIn[0]);
            m_VFX.SetFloat(m_PTurb, f_PTurb * m_SliderIn[1]);
            m_VFX.SetFloat(m_PFreq, f_PFreq * m_SliderIn[2]);
            m_VFX.SetFloat(m_PDrag, f_PTurbDrag * m_SliderIn[3]);
            m_VFX.SetFloat(m_PTurbPosition, f_PTurbPosition * m_SliderIn[5]);

    }
}

