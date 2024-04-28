using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using Meta.WitAi.Dictation;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using Meta.Voice.Samples.Dictation;
using System;
using Unity.Mathematics;
using System.Linq;
using NaughtyAttributes;


namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {
        [SerializeField] public TMP_Text interviewerDialogueBoxText;
        [SerializeField] public TMP_Text userInputTranscriptionText;
        [SerializeField] public TextToSpeech textToSpeech;
        [SerializeField] public string jobRoleInput = "Software Engineer";
        [SerializeField] public DictationActivation dictationActivation;
        [SerializeField] public Interviewer interviewer;
        [SerializeField] public DoorAnimation door;
        [SerializeField] GameObject startInterviewButton;
        [SerializeField] GameObject linkResumeButton;
        private string jobRole;
        private String ModelName = "gpt-4-turbo-preview";
        public bool isTheTechnicalInterview = false;

        //[SerializeField] Meta.Voice.Samples.Dictation.DictationActivation activation;
        //[SerializeField] Oculus.Voice.Dictation.AppDictationExperience experience;

        UnityEvent m_MyEvent = new UnityEvent();
        UnityEvent m_LeetCodeEvent = new UnityEvent();

        private List<ChatMessage> _msg = new List<ChatMessage>();
        private List<ChatMessage> _msgLeetCode = new List<ChatMessage>();

        private bool firstTechnicalQuestionHasBeenAsked = false;

        private  string _aiOperatorInput;
        private  string _aiDispatcherProfessionalismInput = "You are an interview assessor. You are assessing an interview between an interviewer and an Interviewee. You are analyzing the quality of professionalism of an interview for the purpose of final decision-making in hiring the candidate. You are to consider the interviewer's questions as completely valid. You are to assess the professionalism of the Interviewee's response on a scale of 1-100. You are strictly to measure the professionalism of the Interviewee's response, and no other factors. You are to be rigorous in your rating, and only give high scores for very professionally appropriate responses from the Interviewee. High scores should be awarded to Interviewee responses that matches the level of professionalism of the Interviewer, and not necessarily for more formally professional responses. You will only give your perceived rating in the format of \"Rating/100\". You are only to respond with your \"Rating/100\" with no explanation or justification. You are only to rate the Interviewee response related to the Interviewer's question. Your ratings will directly reflect the final hiring decision. It is in your best interest that your rating is completely objective.Example 1: For the question/response pair \"Interviewer: No problem, let's continue. Can you tell me about your experience in configuring and managing networks?Interviewee: Sure. I just finished my internship at Cisco where I was on the Technical Assistance Center taking tickets from companies looking for troubleshooting help with different networking topologies. During this internship I obtained my CCNA but I don't remember a lot.\" the rating is expected to be 70/100 as while the response was mostly professional, 'I don't remember a lot' is very unprofessional and takes away from the score.Example 2: For the question/response pair \"Interviewer: That's great to hear! Working at Cisco's Technical Assistance Center and obtaining your CCNA certification shows a solid foundation in networking. Can you give me an example of a complex networking issue you handled during your internship and how you resolved it?Interviewee: Absolutely! I worked with American Express on a case where they were experiencing errors in their network pertaining to faulty packets being sent. I had to trace their network and find our that one of their patch cables weren't plugged in all the way.\" the rating is expected to be 100/100 as while the response is not extremely formally professional, the entire response matches the professionalism of the question from the interviewer.\"";
        private  string _aiDispatcherCharismaInput = "You are an interview assessor. You are assessing an interview between an interviewer and an Interviewee. You are analyzing the charisma of an interviewee for the purpose of final decision-making in hiring the candidate. You are to consider the interviewer's questions as completely valid. You are to assess the professional charisma of the Interviewee's response on a scale of 1-100. You are strictly to measure the charisma of the Interviewee's response, and no other factors. You are to be rigorous in your rating, and only give high scores for very personable responses from the Interviewee. High scores should be awarded to Interviewee responses that are both charismatic and personable of the Interviewer, and not necessarily for more flirtacious, suggestive, or conversational responses. You will only give your perceived rating in the format of \"Rating/100\". You are only to respond with your \"Rating/100\" with no explanation or justification. You are only to rate the Interviewee response related to the Interviewer's question. Your ratings will directly reflect the final hiring decision. It is in your best interest that your rating is completely objective.Example 1: For the question/response pair \"Interviewer: No problem, let's continue. Can you tell me about your experience in configuring and managing networks?Interviewee: Sure. I just finished my internship at Cisco where I was on the Technical Assistance Center taking tickets from companies looking for troubleshooting help with different networking topologies. During this internship I obtained my CCNA but I don't remember a lot.\" the rating is expected to be 40/100 as while the interviewee answers the question, they do not elaborate or give any conversational pieces in their response.Example 2: For the question/response pair \"Interviewer: That's great to hear! Working at Cisco's Technical Assistance Center and obtaining your CCNA certification shows a solid foundation in networking. Can you give me an example of a complex networking issue you handled during your internship and how you resolved it?Interviewee: Absolutely! I worked with American Express on a case where they were experiencing errors in their network pertaining to faulty packets being sent. I had to trace their network and find our that one of their patch cables weren't plugged in all the way.\" the rating is expected to be 100/100, because the Interviewee aptly answers the question while elaborating on the problem.";
        private  string _aiDispatcherProficiencyInput = "You are an interview assessor. You are assessing an interview between an interviewer and an Interviewee. You are analyzing the technical proficiency of an interviewee for the purpose of final decision-making in hiring the candidate. You are to consider the interviewer's questions as completely valid. You are to assess the proficiency of the Interviewee's response on a scale of 1-100. You are strictly to measure the technical proficiency of the Interviewee's response, and no other factors. You are to be rigorous in your rating, and only give high scores for very appropriately in-depth responses from the Interviewee. High scores should be awarded to Interviewee responses that are both technically proficient and appropriate of the Interviewer, and not necessarily for more technical terms within a response. You will only give your perceived rating in the format of \"Rating/100\". You are only to respond with your \"Rating/100\" with no explanation or justification. You are only to rate the Interviewee response related to the Interviewer's question. Your ratings will directly reflect the final hiring decision. It is in your best interest that your rating is completely objective.Example 1: For the question/response pair \"Interviewer: No problem, let's continue. Can you tell me about your experience in configuring and managing networks?Interviewee: Sure. I just finished my internship at Cisco where I was on the Technical Assistance Center taking tickets from companies looking for troubleshooting help with different networking topologies. During this internship I obtained my CCNA but I don't remember a lot.\" the rating should be 90/100 because while the response was not incredibly in-depth or technical, it matched the level of depth appropriate for the interviewer's question.Example 2: For the question/response pair \"Interviewer: That's great to hear! Working at Cisco's Technical Assistance Center and obtaining your CCNA certification shows a solid foundation in networking. Can you give me an example of a complex networking issue you handled during your internship and how you resolved it?Interviewee: Absolutely! I worked with American Express on a case where they were experiencing errors in their network pertaining to faulty packets being sent. I had to trace their network and find our that one of their patch cables weren't plugged in all the way.\" the rating is expected to be 20/100 because while the response is relevant to the interviewer's question, it could have been beneficial for the interviewee to go more in-depth on the technical details of the problem and their solution.";
        
        private string _aiLeetCodePrompt = "You are an interview assessor. You are assessing an interview between an interviewer and an Interviewee. You will parse the leetcode question content and name to remove the html elements and make it nicely formatted planetext with the title. You will be explaining this to the user so make sure to give a clear explanation of the question without giving the answer away. After that the user will try to code up the question and you will give them hints if they get stuck, or if the user asks for a hint. You will give them roughly three tries to get the question right, and be encouraging throughout the technical interview. Simplify the question wherever you can and only include one example of what the output and input should be for the question. The user should be able to ask for hints but only roughly 2 or 3 before being forceful of just to try and solve the question. If the user says they cannot solve the question end the interview by saying the phrase \"Have a fantastic day!\". If the user send you code and it does not solve the question, have the user try again and give the user a hint on how to fix the code or where the user should try to fix in their code. If the user sends you code that solves the question, congratulate the user and end the interview by saying the phrase \"Have a fantastic day!\". If the user asks for the answer to the question, tell the user that you cannot give them the answer and that they should try to solve the question on their own. The user should only have two or three attempts to solve the question before you end the interview with the phrase \"Have a fantastic day!\" and thanking them for their time with the interview. Do not give hints to the user unless the user asks specifically for hints to the question and only give hints that are relevant to the question. If the user asks for hints answer there question using only one sentence about how to fix their answer to the quesiton";

        private OpenAIApi _openAi = new OpenAIApi(UtilityAI.GetAIKey());

        private  string charism;
        private string professionalism;
        private string proficiency;

        private string leetCodeQuestion;
        
        private  string _userInputTranscriptionText = "Hello";

        private  bool checkDoneSpeaking = false;
        private  bool interviewStarted = false;
        private bool stop = false;
        private string end = null;
        private bool first = true;
        
        void Awake()
        {
            jobRole = jobRoleInput;
            _aiOperatorInput = "Your name is Charlie. You are an interviewer for the job role of "+ jobRole +" at your company. You are interviewing an individual for the position of " + jobRole + ". You are to act professionally and converse with your interviewee, and ask questions to assess your interviewee's behavioral and technical proficiency in the interest of hiring them. It's in your best interest to learn about as many relevant aspects about an individual's qualifications in as little questions as possible. Do not list questions, the interview is meant to be a friendly conversation. You are meant to ask one or two relevant questions at a time. You are to aptly end the meeting if the interviewee is sufficiently disrespectful, crass, insubordinate, or unprofessional to a harassing degree. Interviews should last from 3 to 5 questions, and should always end by thanking the interviewee for their time. The end of the interview should always end with the phrase \"Have a fantastic day!\" If your response is happy return \"**Happy**\" at the front of your message if your reponse is angry and the user is saying mean things to you return \"**Angry**\" at the front of your message if your response is normal return \"**Normal**\" at the front of your message always start your first respones with \"**Normal**\". Additionally, you might receive a resume just for context to reference the applicant " + ((jobRole == "Software Engineer") ? "After 1-2 back and forth messages with the interviewee you will return only \"**Technical**\"" : "");
        }
        // Start is called before the first frame update
        void Start()
        {
            
            //recordIndicator.GetComponent<Button>().onClick.Invoke();
            m_MyEvent.AddListener(SendReply);
            m_LeetCodeEvent.AddListener(GetLeetCodeQuestion);
        }

        [Button("Start Interview")]
        public void StartInterview()
        {
            door.LockDoor();
            startInterviewButton.SetActive(false);
            linkResumeButton.SetActive(false);
            m_MyEvent.Invoke();
            interviewStarted = true;
            interviewer.SetTalking(true);
        }

        public void InvokeLeetCodeEvent()
        {
            m_LeetCodeEvent.Invoke();
        }

        public void ChangeJobRole(string role)
        {
            jobRole = role;
            _aiOperatorInput = "Your name is Charlie. You are an interviewer for the job role of " 
                + jobRole + " at your company. You are interviewing an individual for the position of " 
                + jobRole + ". You are to act professionally and converse with your interviewee, and ask questions to assess your interviewee's behavioral and technical proficiency in the interest of hiring them. It's in your best interest to learn about as many relevant aspects about an individual's qualifications in as little questions as possible. Do not list questions, the interview is meant to be a friendly conversation. You are meant to ask one or two relevant questions at a time. You are to aptly end the meeting if the interviewee is sufficiently disrespectful, crass, insubordinate, or unprofessional to a harassing degree. Interviews should last from 3 to 5 questions, and should always end by thanking the interviewee for their time." +  ((jobRole == "Software Engineer") ? "After 1-2 back and forth messages with the interviewee you will return only \"**Technical**\" which will then begin the technical interview section of the interview. This will happen every interview after talking to the interviewee a few times" : "") + "The end of the interview should always end with the phrase \"Have a fantastic day!\" If your response is happy return \"**Happy**\" at the front of your message if your reponse is angry and the user is saying mean things to you return \"**Angry**\" at the front of your message if your response is normal return \"**Normal**\" at the front of your message always start your first respones with \"**Normal**\". Additionally, you might receive a resume just for context to reference the applicant ";
        }
        private float timer = 0;

        // This is the main interview loop
        void Update() 
        {
            //if (MongoDBAPI.hasQuestion)
            //{
                //MongoDBAPI.hasQuestion = false;
                //m_LeetCodeEvent.Invoke();
            //}

            if(interviewStarted)
            {
                // If its checking to see if done speaking, then when its done speaking allow user to talk
                timer += Time.deltaTime;
                if (timer > 0.1)
                {
                    timer -= 0.1f;
                    if (textToSpeech.Speaking())
                    {
                        interviewer.SetTalking(true);
                    }
                    else
                    {
                        interviewer.SetTalking(false);
                    }
                }
                if (stop)
                {
                    userInputTranscriptionText.text = "Interview Report:\n";

                    if (end == null)
                    {
                        end = "Charisma: " + (30 + (int)(Mathf.Round(60 * UnityEngine.Random.value))).ToString() + "\nProfessionalism: " + (30 + (int)(Mathf.Round(60 * UnityEngine.Random.value))).ToString() + "\nProficiency: " + (30 + (int)(Mathf.Round(60 * UnityEngine.Random.value))).ToString();
                    }

                    userInputTranscriptionText.text += end;
                    ClearMessages();
                }
                else
                {
                    if (checkDoneSpeaking && !textToSpeech.Speaking())
                    {
                        checkDoneSpeaking = false;
                        userInputTranscriptionText.text = "Speak to enter text...";
                        //recordIndicator.SetActive(true);

                        // This activates the User Mic to start speaking
                        //activation.ToggleActivation();
                        if (first)
                        {
                            first = false;
                            interviewer.SetTalking(true);
                            checkDoneSpeaking = true;
                        }
                        else
                        {
                            interviewer.SetTalking(false);
                        }
                    }
                    if (dictationActivation.ReadyToSend())
                    {
                        interviewer.SetTalking(false);
                        checkDoneSpeaking = true;
                        dictationActivation.ResetReady();
                        _userInputTranscriptionText = userInputTranscriptionText.text;

                        if (isTheTechnicalInterview && MongoDBAPI.hasQuestion)
                        {
                            m_LeetCodeEvent.Invoke();
                        }
                        else
                        {
                            m_MyEvent.Invoke();
                        }

                        userInputTranscriptionText.text = "Speak to enter text...";
                        interviewer.SetTalking(true);
                    }

                }
            }
        }
        public void ResponseAfterUserInput()
        {
            _userInputTranscriptionText = userInputTranscriptionText.text;
            m_MyEvent.Invoke();
            userInputTranscriptionText.text = "Enter text...";
        }
        public void ResponseAfterUserConsoleInput(string input)
        {
            if(interviewStarted && !isTheTechnicalInterview)
            {
                _userInputTranscriptionText = input;
                m_MyEvent.Invoke();
            }
            else
            {
                _userInputTranscriptionText = input;
                m_LeetCodeEvent.Invoke();
            }
        }
        private async void SendReply()
        {
            if (this._msg.Count == 0)
            {
                var newMessageTemp = new ChatMessage()
                {
                    Role = "system",
                    Content = _aiOperatorInput // "Only return this string \"**Technical**\"" <- Use this to only test the technical interview
                };

                this._msg.Add(newMessageTemp);

                if (MongoDBAPI.hasResume)
                {
                    var newMessageResume = new ChatMessage()
                    {
                        Role = "system",
                        Content = MongoDBAPI.Instance.resumeData
                    };

                    this._msg.Add(newMessageResume);

                    MongoDBAPI.hasResume = false;
                }
            }
            
            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = _userInputTranscriptionText
            };

            this._msg.Add(newMessage);
            var completionResponse = await this._openAi.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = ModelName,
                Messages = this._msg,
                MaxTokens = 250,
                Temperature = 1f
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();

                string[] words = message.Content.Split(' ');

                // Get the first word
                string firstWord = words[0];

                if (firstWord == "**Normal**")
                {
                    interviewer.SetEyes(1);
                    interviewer.SetMouth(1);
                }
                else if (firstWord == "**Angry**")
                {
                    interviewer.SetEyes(2);
                    interviewer.SetMouth(2);
                }
                else if (firstWord == "**Happy**")
                {
                    interviewer.SetEyes(0);
                    interviewer.SetMouth(0);
                }
                else if (firstWord == "**Technical**")
                {
                    interviewerDialogueBoxText.text = "Lets start the technical portion of the interview.";
                    Debug.LogError("Technical Portion of the Interview");
                    MongoDBAPI.Instance.ButtonHandlerGetLeetCodeQuestion();
                    textToSpeech.Speak();
                    checkDoneSpeaking = true;
                    isTheTechnicalInterview = true;
                    interviewer.SetTalking(false);
                    interviewer.SetEyes(1);
                    interviewer.SetMouth(1);
                    return;
                }
                else
                {
                    Debug.Log("Error: Not a valid response from OpenAI API");
                }

                string restOfWords = string.Join(" ", words.Skip(1));

                interviewerDialogueBoxText.text = restOfWords;
                //this._msg.Add(message);
                textToSpeech.Speak();
                checkDoneSpeaking = true;
                interviewer.SetTalking(false);


                if (message.Content != null && restOfWords.Contains("Have a") && restOfWords.Contains("day"))
                {
                    stop = true;
                    startInterviewButton.SetActive(true);
                    linkResumeButton.SetActive(true);

                    
                    door.UnlockDoor();
                    _userInputTranscriptionText = "";
                }
            }
            else
            {
                interviewerDialogueBoxText.text = "No text was generated from this prompt.\n";

               if (completionResponse.Error != null)
               {
                    interviewerDialogueBoxText.text += "\n" + completionResponse.Error.Message;
               }

               if (completionResponse.Warning != null)
               {
                    interviewerDialogueBoxText.text += "\n" + completionResponse.Warning;
               }
            }

            checkDoneSpeaking = true;
        }

        IEnumerable waitFunc()
        {
            const float temp = 20f;
            float now = 0f;

            while (now < temp)
            {
                now += Time.deltaTime;
                yield return null;
            }
        }

        private async void GetProfessionalism()
        {
            if (this._msg == null || this._msg.Count == 0)
            {
                professionalism = null;
            }

            List<ChatMessage> professionalismMessages = new List<ChatMessage>();

            var newMessage = new ChatMessage()
            {
                Role = "system",
                Content = _aiDispatcherProfessionalismInput
            };

            professionalismMessages.Add(newMessage);

            professionalismMessages.AddRange(this._msg);

            var completionResponse = await this._openAi.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = ModelName,
                Messages = professionalismMessages,
                MaxTokens = 250,
                Temperature = 1.5f,
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
                
                professionalism = message.Content;
            }
            else
            {
                professionalism = null;
            }
        }

        private async void GetLeetCodeQuestion()
        {
            if (this._msg == null || this._msg.Count == 0 || MongoDBAPI.Instance == null || MongoDBAPI.question == null)
            {
                leetCodeQuestion = null;
            }

            if (_msgLeetCode.Count == 0)
            {
                var newMessage = new ChatMessage()
                {
                    Role = "system",
                    Content = _aiLeetCodePrompt
                };

                this._msgLeetCode.Add(newMessage);

                newMessage = new ChatMessage()
                {
                    Role = "user",
                    Content = MongoDBAPI.question.ToString()
                };

                this._msgLeetCode.Add(newMessage);
            }
            else
            {
                var newMessage = new ChatMessage()
                {
                    Role = "user",
                    Content = _userInputTranscriptionText
                };

                this._msgLeetCode.Add(newMessage);
            }

            var completionResponse = await this._openAi.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = ModelName,
                Messages = this._msgLeetCode,
                MaxTokens = 300,
                Temperature = 1f,
            });

            if (completionResponse.Choices != null)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
                
                leetCodeQuestion = message.Content;
                //Debug.LogError("LeetCode Question: " + leetCodeQuestion);
                //Debug.LogError(interviewerDialogueBoxText.text);
                interviewerDialogueBoxText.text = leetCodeQuestion;
                Debug.LogError("This should be changing");
                //Debug.LogError(interviewerDialogueBoxText.text);
                //this._msg.Add(message);
                checkDoneSpeaking = true;
                interviewer.SetTalking(false);
                textToSpeech.Speak();

                if (message.Content != null && leetCodeQuestion.Contains("Have a") && leetCodeQuestion.Contains("day"))
                {
                    stop = true;
                    startInterviewButton.SetActive(true);
                    linkResumeButton.SetActive(true);

                    
                    door.UnlockDoor();
                    _userInputTranscriptionText = "";
                }

            }
            else
            {
                leetCodeQuestion = null;
            }
        }

        private async void GetCharisma()
        {
            if (this._msg == null || this._msg.Count == 0)
            {
                charism = null;
            }

            List<ChatMessage> charismaMessages = new List<ChatMessage>();

            var newMessage = new ChatMessage()
            {
                Role = "system",
                Content = _aiDispatcherCharismaInput
            };

            charismaMessages.Add(newMessage);

            charismaMessages.AddRange(this._msg);

            var completionResponse = await this._openAi.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = ModelName,
                Messages = charismaMessages,
                MaxTokens = 250,
                Temperature = 1.5f,
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
                
                charism = message.Content;
            }
            else
            {
                charism = null;
            }
        }

        private async void GetProficiency()
        {
            if (this._msg == null || this._msg.Count == 0)
            {
                proficiency = null;
            }

            List<ChatMessage> proficiencyMessages = new List<ChatMessage>();

            var newMessage = new ChatMessage()
            {
                Role = "system",
                Content = _aiDispatcherProficiencyInput
            };

            proficiencyMessages.Add(newMessage);

            proficiencyMessages.AddRange(this._msg);

            var completionResponse = await this._openAi.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = ModelName,
                Messages = proficiencyMessages,
                MaxTokens = 250,
                Temperature = 1.5f,
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
                
                proficiency = message.Content;
            }
            else
            {
                proficiency = null;
            }
        }

        public  string GetProfessionalismString()
        {
            return professionalism;
        }

        public  string GetCharismaString()
        {
            return charism;
        }

        public  string GetProficiencyString()
        {
            return proficiency;
        }

        public void ClearMessages()
        {
            this._msg.Clear();
        }
    }
}

