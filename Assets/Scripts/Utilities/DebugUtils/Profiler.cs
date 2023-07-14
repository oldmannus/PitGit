using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;   // needed for stopwatch


namespace Pit.Utilities
{
    public class Profiler
    {
        // public functions
        #region Public
            public static void Start( string t )    { mInstance.AddInternal(t); }
            public static void Stop( string t )     { mInstance.FinishInternal(t); }
            public static void Update()             { mInstance.IncrementFrameCount(); } // call once per frame
           // public static void Clear()              { mInstance.ClearInternal(); }
            public static void Dump()               { mInstance.DumpInternal(); }
            public static float GetFPS()            { return mInstance.FPS; }
        #endregion



        #region Private
            const long kTimePerFPSCheck = 250;    // in milliseconds

            // static singleton object
            private static Profiler mInstance = new Profiler();

            struct ProfileSample
            {
               public bool     Valid;                  //Whether this data is valid
               public int ProfileInstances;       //# of times ProfileBegin called
               public int OpenProfiles;           //# of times ProfileBegin w/o ProfileEnd
               public string Name;                   //Name of sample
               public float StartTime;              //The current open profile start time
               public float Accumulator;            //All samples this frame added together
               public float ChildrenSampleTime;     //Time taken by all children
               public int NumParents;             //Number of profile parents
            };


            struct ProfileSampleHistory
            {
               public bool Valid;          //Whether the data is valid
               public string Name;         //Name of the sample
               public float Ave;           //Average time per frame (percentage)
               public float Min;           //Minimum time per frame (percentage)
               public float Max;           //Maximum time per frame (percentage)
            } ;

        

            const int NUM_PROFILE_SAMPLES  = 50;
            ProfileSample[] Samples = new ProfileSample[NUM_PROFILE_SAMPLES];
            ProfileSampleHistory[] History = new ProfileSampleHistory[NUM_PROFILE_SAMPLES];
            float StartProfile = 0.0f;
            float EndProfile = 0.0f;


            private Stopwatch               mStopwatch;
            private long                    mTimeOfStartFPSCheck;
            private int                     mNumFramesThisFPSCheck;
            private float                   FPS { get; set; }
            private long                    CurTime { get { return mStopwatch.ElapsedTicks; } }
      

       
            // ----------------------------------------------------------------------------------
            // Constructor
            // ----------------------------------------------------------------------------------
            public Profiler()
            {
                Dbg.Assert( mInstance == null );


                for( int i=0; i<NUM_PROFILE_SAMPLES; i++ ) 
                {
                    Samples[i].Valid = false;
                    History[i].Valid = false;
                }



                mInstance = this;
                mStopwatch = new Stopwatch();
                mStopwatch.Start();
                mNumFramesThisFPSCheck = 0;
                mTimeOfStartFPSCheck = 0;
            }

            // ----------------------------------------------------------------------------------
            // this is inefficent for large numbers of entry points. 
            // if we have lots of entry points, then I need to fix
            // ----------------------------------------------------------------------------------
            private void AddInternal(string name )
            {
                int i = 0;

                while( i < NUM_PROFILE_SAMPLES && Samples[i].Valid == true ) 
                {
                    if( Samples[i].Name == name )  
                    {
                        //Found the sample
                        Samples[i].OpenProfiles++;
                        Samples[i].ProfileInstances++;
                        Samples[i].StartTime = mStopwatch.ElapsedMilliseconds;
                        Dbg.Assert( Samples[i].OpenProfiles == 1 ); //max 1 open at once
                        return;
                   }
                   i++;	
                }

                if( i >= NUM_PROFILE_SAMPLES ) 
                {
                    Dbg.Assert( false );  // too many samples
                    return;
                }

                Samples[i].Name = name ;
                Samples[i].Valid = true;
                Samples[i].OpenProfiles = 1;
                Samples[i].ProfileInstances = 1;
                Samples[i].Accumulator = 0.0f;
                Samples[i].StartTime = mStopwatch.ElapsedMilliseconds;
                Samples[i].ChildrenSampleTime = 0.0f;

            }

            // ----------------------------------------------------------------------------------
            // Adds the time between now and when we were first called
            // ----------------------------------------------------------------------------------
             private void FinishInternal(string name)
            {
                int i = 0;
                int numParents = 0;

                while( i < NUM_PROFILE_SAMPLES && Samples[i].Valid == true )
                {
                    if( Samples[i].Name ==name ) 
                    {  
                        //Found the sample
                        int inner = 0;
                        int parent = -1;
                        float fEndTime = mStopwatch.ElapsedMilliseconds;
                        Samples[i].OpenProfiles--;

                        //Count all parents and find the immediate parent
                        while( Samples[inner].Valid == true ) 
                        {
                            if( Samples[inner].OpenProfiles > 0 )
                            {  //Found a parent (any open profiles are parents)
                                numParents++;
                                if( parent < 0 )
                                {
                                    parent = inner;//Replace invalid parent (index)
                                }
                                else if( Samples[inner].StartTime >= Samples[parent].StartTime )
                                {
                                    parent = inner;//Replace with more immediate parent
                                }
                            }
                            inner++;
                        }

                        //Remember the current number of parents of the sample
                        Samples[i].NumParents = numParents;

                        if( parent >= 0 )
                        {  //Record this time in fChildrenSampleTime (add it in)
                            Samples[parent].ChildrenSampleTime += fEndTime -Samples[i].StartTime;
                        }

                        //Save sample time in accumulator
                        Samples[i].Accumulator += fEndTime - Samples[i].StartTime;
                        return;
                    }

                    i++;	
                }
            }
      
      
            // ----------------------------------------------------------------------------------
            // Call this every frame to get useful frame averages
            // ----------------------------------------------------------------------------------
            private void IncrementFrameCount()
            {

                mNumFramesThisFPSCheck++;

                long curMS = mStopwatch.ElapsedMilliseconds;

                if (curMS > mTimeOfStartFPSCheck + kTimePerFPSCheck)
                {
                    float seconds = (curMS - mTimeOfStartFPSCheck) / 1000.0f;
                    FPS = mNumFramesThisFPSCheck / seconds;
                    mTimeOfStartFPSCheck = curMS;
                    mNumFramesThisFPSCheck = 0;
                }

/*
                long now = mStopwatch.ElapsedMilliseconds;

                float elapsedMS = now - mTimeOfStartFPSCheck;
                FPS = 1000 / elapsedMS;
                mTimeOfStartFPSCheck = now;
 */ 
            }


            // ----------------------------------------------------------------------------------
            // Dumps profile information
            // ----------------------------------------------------------------------------------
            private void DumpInternal()
            {
                int i = 0;

                EndProfile = mStopwatch.ElapsedMilliseconds;

                if (StartProfile != EndProfile)
                {

                    Dbg.Log("  Ave :   Min :   Max :   # : Profile Name");
                    Dbg.Log("--------------------------------------------");

                    while (i < NUM_PROFILE_SAMPLES && Samples[i].Valid == true)
                    {
                        int indent = 0;
                        float sampleTime, percentTime, aveTime, minTime, maxTime;
                        string line, name, indentedName;
                        string ave, min, max, num;

                        if (Samples[i].OpenProfiles < 0)
                        {
                            Dbg.Assert(false);    //"ProfileEnd() called without a ProfileBegin()" 	
                        }
                        else if (Samples[i].OpenProfiles > 0)
                        {
                            Dbg.Assert(false); // ( !"ProfileBegin() called without a ProfileEnd()" );
                        }

                        Dbg.Assert(EndProfile != StartProfile);

                        sampleTime = Samples[i].Accumulator - Samples[i].ChildrenSampleTime;
                        percentTime = (sampleTime / (EndProfile - StartProfile)) * 100.0f;

                        aveTime = minTime = maxTime = percentTime;

                        //Add new measurement into the history and get the ave, min, and max
                        StoreProfileInHistory(Samples[i].Name, percentTime);
                        GetProfileFromHistory(Samples[i].Name, ref aveTime, ref minTime, ref maxTime);

                        //Format the data
                        ave = String.Format("{0:00.00}", aveTime);    //### fix formatting
                        min = String.Format("{0:00.00}", minTime);    //### fix formatting
                        max = String.Format("{0:00.00}", maxTime);    //### fix formatting
                        num = String.Format("{0:0000.00}", Samples[i].ProfileInstances);

                        indentedName = Samples[i].Name;

                        for (indent = 0; indent < Samples[i].NumParents; indent++)
                        {
                            name = "   " + indentedName;
                            indentedName = name;
                        }

                        line = ave + " : " + min + " : " + max + " : " + num + " : " + indentedName;
                        Dbg.Log(line);
                        i++;
                    }

                    Dbg.Log(" ");   // newline
                }
        
                //Reset samples for next frame
                for( int ii=0; ii<NUM_PROFILE_SAMPLES; ii++ ) 
                {
                    Samples[ii].Valid = false;
                }
                StartProfile =  mStopwatch.ElapsedMilliseconds;
                
            }



            void StoreProfileInHistory(string name, float percent)
            {
                int i = 0;
                float oldRatio;
                float newRatio = 0.8f * mStopwatch.ElapsedMilliseconds;
                if( newRatio > 1.0f ) 
                {
                    newRatio = 1.0f;
                }
                
                oldRatio = 1.0f - newRatio;

                while( i < NUM_PROFILE_SAMPLES && History[i].Valid == true ) 
                {
                    if( History[i].Name == name ) 
                    {  //Found the sample
                        History[i].Ave = (History[i].Ave*oldRatio) + (percent*newRatio);
                        if( percent < History[i].Min ) 
                            History[i].Min = percent;
                        else 
                            History[i].Min = (History[i].Min*oldRatio) + (percent*newRatio);
                     
                        if( History[i].Min < 0.0f ) 
                            History[i].Min = 0.0f;
                     

                        if( percent > History[i].Max ) 
                            History[i].Max = percent;
                        else
                            History[i].Max = (History[i].Max*oldRatio) + (percent*newRatio);
                        return;
                    }
                    i++;
                }

                if( i < NUM_PROFILE_SAMPLES )
                {  
                    //Add to history
                    History[i].Name = name ;
                    History[i].Valid = true;
                    History[i].Ave = History[i].Min = History[i].Max = percent;
                }
                else 
                {
                    Dbg.Assert(false);    //Exceeded Max Available Profile Samples!
                }
            }

            void GetProfileFromHistory(string name, ref float ave, ref float min, ref float max)
            {
                int i = 0;
                while( i < NUM_PROFILE_SAMPLES && History[i].Valid == true ) 
                {
                    if( History[i].Name == name )
                    {  //Found the sample
                        ave = History[i].Ave;
                        min = History[i].Min;
                        max = History[i].Max;
                        return;
                    }
                    i++;
                }	
                ave = min = max = 0.0f;
            }



        #endregion
    }

}
