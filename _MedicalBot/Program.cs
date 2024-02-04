using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace _MedicalBot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string errorMessage;
            Patient patient = new Patient();
            MedicalBot medBot = new MedicalBot();

            Console.WriteLine("Hi, I'm " + MedicalBot.GetBotName() + ". I'm here to help you with your medication.");
            Console.WriteLine("Enter your (patient) details: ");

            Console.Write("Enter Patient Name: ");
            do
            {
                string name = Console.ReadLine();
                if (patient.SetName(name, out errorMessage))
                {
                    break; // Break the loop if the input is valid
                }
                Console.WriteLine($"Error: {errorMessage}");
                Console.Write("Enter Patient Name: ");
            } while (true);

            Console.Write("Enter Patient Age: ");
            do
            {
                if (int.TryParse(Console.ReadLine(), out int age) && patient.SetAge(age, out errorMessage))
                {
                    break;
                }
                Console.WriteLine($"Error: {errorMessage}");
                Console.Write("Enter Patient Age: ");
            } while (true);

            Console.Write("Enter Patient Gender: ");
            do
            {
                string gender = Console.ReadLine();
                if (patient.SetGender(gender, out errorMessage))
                {
                    break;
                }
                Console.WriteLine($"Error: {errorMessage}");
                Console.Write("Enter Patient Gender: ");
            } while (true);

            Console.Write("Enter Medical History. Eg: Diabetes. Press Enter for None: ");
            string medicalHistory = Console.ReadLine();
            patient.SetMedicalHistory(medicalHistory);

            Console.Clear();
            Console.WriteLine($"Welcome, {patient.GetName()}, {patient.GetAge()}.");

            Console.WriteLine("Which of the following symptoms do you have?\n" +
                              "S1. Headache\n" +
                              "S2. Skin rashes\n" +
                              "S3. Dizziness");
            Console.WriteLine();

            Console.Write("Enter the symptom code from the above list (S1, S2, or S3): ");
            do
            {
                string symptomCode = Console.ReadLine();
                if (patient.SetSymptomCode(symptomCode, out errorMessage))
                {
                    break;
                }
                Console.WriteLine($"Error: {errorMessage}");
                Console.Write("Enter the symptom code from the above list (S1, S2, or S3): ");
            } while (true);

            Console.WriteLine("Your prescription based on your age, symptoms, and medical history:");
            medBot.PrescribeMedication(patient);
            Console.Clear();
            Console.WriteLine("Prescription: " + patient.GetPrescription());
            Console.WriteLine("\nThank you for coming.");

            Console.ReadKey();
        }
    }

    class MedicalBot
    {
        const string BotName = "Bob";

        internal static string GetBotName()
        {
            return BotName;
        }

        internal void PrescribeMedication(Patient patient)
        {
            string medicineName = "";

            if (patient.GetSymptoms().ToLower() == "headache")
                medicineName = "ibuprofen";
            else if (patient.GetSymptoms().ToLower() == "skin rashes")
                medicineName = "diphenhydramine";
            else if (patient.GetSymptoms().ToLower() == "dizziness")
            {
                if (patient.GetMedicalHistory().ToLower() == "diabetes")
                    medicineName = "metformin";
                else
                    medicineName = "dimenhydrinate";
            }

            string dosage = GetDosage(patient.GetAge(), medicineName);
            patient.SetPrescription($"{medicineName} {dosage}");
        }

        string GetDosage(int patientAge, string medicineName)
        {
            string dosage = "";

            if (medicineName == "ibuprofen")
            {
                if (patientAge < 18)
                    dosage = "400 mg";
                else
                    dosage = "800 mg";
            }
            else if (medicineName == "diphenhydramine")
            {
                if (patientAge < 18)
                    dosage = "50 mg";
                else
                    dosage = "300 mg";
            }
            else if (medicineName == "dimenhydrinate")
            {
                if (patientAge < 18)
                    dosage = "50 mg";
                else
                    dosage = "400 mg";
            }
            else if (medicineName == "metformin")
            {
                dosage = "500 mg";
            }
            return dosage;
        }
    }

    class Patient
    {
        private string symptoms;
        private bool diabetesHistory;

        private string _name;
        private int _age;
        private string _gender;
        private string _medicalHistory;
        private string _symptomCode;
        private string _prescription;

        internal string GetName()
        {
            return _name;
        }
        internal bool SetName(string name, out string errorMessage)
        {
            errorMessage = "";
            bool valid = false;
            if (!string.IsNullOrEmpty(name) && name.Length >= 2)
            {
                _name = name;
                valid = true;
            }
            else
            {
                valid = false;
                errorMessage = "Your name is invalid!";
            }
            return valid;
        }


        internal int GetAge()
        {
            return _age;
        }
        internal bool SetAge(int age, out string errorMessage)
        {
            errorMessage = "";
            bool valid = false;
            if (age > 0 && age <= 100)
            {
                _age = age;
                valid = true;
            }
            else
            {
                valid = false;
                errorMessage = "Your age is invalid!";
            }
            return valid;
        }


        internal string GetGender()
        {
            return _gender;
        }
        internal bool SetGender(string gender, out string errorMessage)
        {
            errorMessage = "";
            bool valid = false;
            if (gender == "Male" || gender == "Female" || gender == "Other")
            {
                _gender = gender;
                valid = true;
            }
            else
            {
                valid = false;
                errorMessage = "Your gender is invalid!";
            }
            return valid;
        }


        internal string GetMedicalHistory()
        {
            return _medicalHistory;
        }
        internal void SetMedicalHistory(string medicalHistory)
        {
            _medicalHistory = medicalHistory;
        }


        internal bool SetSymptomCode(string symptomCode, out string errorMessage)
        {
            errorMessage = "";
            bool valid = false;
            if (symptomCode == "S1" || symptomCode == "S2" || symptomCode == "S3")
            {
                _symptomCode = symptomCode;
                valid = true;
            }
            else
            {
                valid = false;
                errorMessage = "The symptom code is invalid!";
            }
            return valid;
        }

        internal string GetSymptoms()
        {
            if (_symptomCode.ToLower() == "s1")
                return symptoms = "Headache";
            else if (_symptomCode.ToLower() == "s2")
                return symptoms = "Skin rashes";
            else if (_symptomCode.ToLower() == "s3")
                return symptoms = "Dizziness";
            else
                return symptoms = "Unknown";
        }


        internal string GetPrescription()
        {
            return _prescription;
        }
        internal void SetPrescription(string prescription)
        {
            _prescription = prescription;
        }
    }
}
