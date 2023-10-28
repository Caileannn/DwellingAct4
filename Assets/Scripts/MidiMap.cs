using Minis;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

[ExecuteInEditMode]
[System.Serializable]
public class MidiMap : MonoBehaviour
{
    public VisualEffect m_VFX;

    // Point VFX Parameters
    private int m_PLifeMax;
    private int m_PRate;
    private int m_PNoise;
    private int m_PSpeed;
    private int m_PRatio;
    private int m_PGravity;
    private int m_PFreq;
    private int m_PTurb;

    // Line VFX Parameters
    private int m_LToggle;
    private int m_LRate;
    private int m_LLifetime;
    private int m_LRange;
    private int m_LThickness;

    // Bubble VFX Parameters
    private int m_BSize;
    private int m_BRate;
    private int m_BLifetime;
    private int m_BTurb;
    private int m_BFreq;
    private int m_BForce;
    private int m_BToggle;


    public float f_PLifeMax = 20f;
    public float f_PRate = 70000f;
    public float f_PNoise = 5f;
    public float f_PSpeed = 5f;
    public float f_PRatio = 1f;
    public float f_PGravity = 0f;
    public float f_PFreq = 5f;
    public float f_PTurb = 5f;
     
    public float f_LRate = 10000f;
    public float f_LLifetime = 5f;
    public bool f_LToggle = false;
    public float f_LRange = 1f;
    public float f_LThickness = 1f;
     
    public float f_BSize = 1f;
    public float f_BRate = 10000f;
    public float f_BFreq = 10f;
    public float f_BLifetime = 10f;
    public float f_BTurb = 10f;
    public bool f_BToggle = false;
     
    public float f_BForceX = 0f;
    public float f_BForceY = 0f;
    public float f_BForceZ = 0f;
     
    private  float m_Slider = 0f;
    private  bool m_BSlider = false;
    private float m_Potent = 0f;
    private bool m_BPotent = false;


    private float[] m_SliderIn = new float[8];
    private float[] m_PotIn = new float[8];

    void Start()
    {

        m_PLifeMax = Shader.PropertyToID("Point Lifetime Max");
        m_PRate = Shader.PropertyToID("Point Rate");
        m_PRatio = Shader.PropertyToID("Turbulences Ratio");
        m_PNoise = Shader.PropertyToID("Noise");
        m_PSpeed = Shader.PropertyToID("Turbulences Speed");
        m_PFreq = Shader.PropertyToID("Turbulences Frequency");
        m_PGravity = Shader.PropertyToID("Gravity Point");
        m_PTurb = Shader.PropertyToID("Turbulences");

        m_LToggle = Shader.PropertyToID("Toggle Line");
        m_LRate = Shader.PropertyToID("Line Rate");
        m_LLifetime = Shader.PropertyToID("Line Lifetime");
        m_LRange = Shader.PropertyToID("Line Range");
        m_LThickness = Shader.PropertyToID("Line Thickness");

        m_BFreq = Shader.PropertyToID("Bubble Turbulences Frequency");
        m_BRate = Shader.PropertyToID("Bubble Rate");
        m_BLifetime = Shader.PropertyToID("Bubble Lifetime");
        m_BTurb = Shader.PropertyToID("Bubble Turbulences");
        m_BForce = Shader.PropertyToID("Bubble Force");
        m_BToggle = Shader.PropertyToID("Toggle Bubble");
        m_BSize = Shader.PropertyToID("Bubble Size");


        InputSystem.onDeviceChange += (device, change) =>
        {
            if (change != InputDeviceChange.Added) {
              
                return;
            }

            var midiDevice = device as Minis.MidiDevice;

            if (midiDevice == null)
            {
             
                return; }

            midiDevice.onWillControlChange += (device, change) =>
            {
                if(device.controlNumber == 61) 
                {
                    if (m_BSlider)
                    {
                        m_BSlider = false;
                        return;
                    }
                    // Left
                    m_Slider -= 1;
                    if(m_Slider < 0) { m_Slider = 2; }
                    m_BSlider = true;
                }

                if (device.controlNumber == 62)
                {
                    // Right
                    if (m_BSlider)
                    {
                        m_BSlider = false;
                        return;
                    }
                    
                    m_Slider += 1;
                    if (m_Slider > 2) { m_Slider = 0; }
                    m_BSlider = true;
                }

                if (device.controlNumber == 58)
                {
                    if (m_BPotent)
                    {      
                        m_BPotent = false;
                        return;
                    }
                    // Left
                    m_Potent -= 1;
                    if (m_Potent < 0) { m_Potent = 2; }
                    m_BPotent = true;
                }

                if (device.controlNumber == 59)
                {
                    // Right
                    if (m_BPotent)
                    {
                        m_BPotent = false;
                        return;
                    }

                    m_Potent += 1;
                    if (m_Potent > 2) { m_Potent = 0; }
                    m_BPotent = true;
                }



                if (device.controlNumber < 8)
                {
                    m_SliderIn[device.controlNumber] = (float)change;
                }

                if (device.controlNumber >= 16 && device.controlNumber <=23)
                {

                    m_PotIn[device.controlNumber - 16] = (float)change;
                }

                // Debug.Log(device.controlNumber);
                Debug.Log(m_Slider + ":" + m_Potent);

                if (device.controlNumber == 32)
                {
                    f_PGravity = -9.8f * (float)change;
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
        if(m_Slider == 0)
        {
            // Points 
            m_VFX.SetFloat(m_PRate, f_PRate * m_SliderIn[0]);
            m_VFX.SetFloat(m_PLifeMax, f_PLifeMax * m_SliderIn[1]);
            m_VFX.SetFloat(m_PNoise, f_PNoise * m_SliderIn[2]);
            m_VFX.SetFloat(m_PTurb, f_PTurb * m_SliderIn[3]);
            m_VFX.SetFloat(m_PSpeed, f_PSpeed * m_SliderIn[4]);
            m_VFX.SetFloat(m_PRatio, f_PRatio * m_SliderIn[5]);
            m_VFX.SetFloat(m_PSpeed, f_PSpeed * m_SliderIn[6]);
            m_VFX.SetFloat(m_PFreq, f_PFreq * m_SliderIn[7]);
            m_VFX.SetVector3(m_PGravity, new Vector3(0, f_PGravity, 0));

            

        }
        if (m_Slider == 1)
        {
            // Points 
            m_VFX.SetBool(m_LToggle, true);
            m_VFX.SetFloat(m_LRate, f_LRate * m_SliderIn[0]);
            m_VFX.SetFloat(m_LLifetime, f_LLifetime * m_SliderIn[1]);
            m_VFX.SetFloat(m_LRange, f_LRange * m_SliderIn[2]);
            m_VFX.SetFloat(m_LThickness, f_LThickness * m_SliderIn[3]);
            //m_VFX.SetFloat(m_PSpeed, f_PSpeed * m_SliderIn[4]);
            //m_VFX.SetFloat(m_PRatio, f_PRatio * m_SliderIn[5]);
            //m_VFX.SetFloat(m_PSpeed, f_PSpeed * m_SliderIn[6]);
            //m_VFX.SetFloat(m_PFreq, f_PFreq * m_SliderIn[7]);

            


        }
        if (m_Slider == 2)
        {
            m_VFX.SetBool(m_BToggle, true);
            m_VFX.SetFloat(m_BRate, f_BRate * m_SliderIn[0]);
            m_VFX.SetFloat(m_BSize, f_BSize * m_SliderIn[1]);
            m_VFX.SetFloat(m_BFreq, f_BFreq * m_SliderIn[2]);
            m_VFX.SetFloat(m_BLifetime, f_BLifetime * m_SliderIn[3]);
            m_VFX.SetFloat(m_BTurb, f_BTurb * m_SliderIn[4]);
            m_VFX.SetVector3(m_BForce, new Vector3(0, -9.8f * m_SliderIn[5], 0));


        }

        if (m_Potent == 0)
        {
            // Points 
            m_VFX.SetFloat(m_PRate, f_PRate * m_PotIn[0]);
            m_VFX.SetFloat(m_PLifeMax, f_PLifeMax * m_PotIn[1]);
            m_VFX.SetFloat(m_PNoise, f_PNoise * m_PotIn[2]);
            m_VFX.SetFloat(m_PTurb, f_PTurb * m_PotIn[3]);
            m_VFX.SetFloat(m_PSpeed, f_PSpeed * m_PotIn[4]);
            m_VFX.SetFloat(m_PRatio, f_PRatio * m_PotIn[5]);
            m_VFX.SetFloat(m_PSpeed, f_PSpeed * m_PotIn[6]);
            m_VFX.SetFloat(m_PFreq, f_PFreq * m_PotIn[7]);
            m_VFX.SetVector3(m_PGravity, new Vector3(0, f_PGravity, 0));



        }
        if (m_Potent == 1)
        {
            // Points 
            m_VFX.SetBool(m_LToggle, true);
            m_VFX.SetFloat(m_LRate, f_LRate * m_PotIn[0]);
            m_VFX.SetFloat(m_LLifetime, f_LLifetime * m_PotIn[1]);
            m_VFX.SetFloat(m_LRange, f_LRange * m_PotIn[2]);
            m_VFX.SetFloat(m_LThickness, f_LThickness * m_PotIn[3]);
            //m_VFX.SetFloat(m_PSpeed, f_PSpeed * m_SliderIn[4]);
            //m_VFX.SetFloat(m_PRatio, f_PRatio * m_SliderIn[5]);
            //m_VFX.SetFloat(m_PSpeed, f_PSpeed * m_SliderIn[6]);
            //m_VFX.SetFloat(m_PFreq, f_PFreq * m_SliderIn[7]);




        }
        if (m_Potent == 2)
        {
            m_VFX.SetBool(m_BToggle, true);
            m_VFX.SetFloat(m_BRate, f_BRate * m_PotIn[0]);
            m_VFX.SetFloat(m_BSize, f_BSize * m_PotIn[1]);
            m_VFX.SetFloat(m_BFreq, f_BFreq * m_PotIn[2]);
            m_VFX.SetFloat(m_BLifetime, f_BLifetime * m_PotIn[3]);
            m_VFX.SetFloat(m_BTurb, f_BTurb * m_PotIn[4]);
            m_VFX.SetVector3(m_BForce, new Vector3(0, -9.8f * m_PotIn[5], 0));


        }
    }
}
