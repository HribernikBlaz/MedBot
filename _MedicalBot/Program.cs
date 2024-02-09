using System;
using System.Collections.Generic;
using System.Linq;

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
            Console.WriteLine("-------Enter your (patient) details-------");

            Console.Write("Enter Patient Name: ");
            do
            {
                string name = Console.ReadLine();
                if (patient.SetName(name, out errorMessage))
                {
                    break;
                }
                Console.WriteLine($"Error: {errorMessage}");

                Console.WriteLine("--------------------");
                Console.Write("Enter Patient Name: ");
            } while (true);

            Console.WriteLine("Enter Patient Surname: ");

            Console.Write("Enter Patient Age: ");
            do
            {
                if (int.TryParse(Console.ReadLine(), out int age) && patient.SetAge(age, out errorMessage))
                {
                    break;
                }
                Console.WriteLine($"Error: {errorMessage}");
                Console.WriteLine("--------------------");
                Console.Write("Enter Patient Age: ");
            } while (true);

            Console.Write("Enter Patient Gender: ");
            do
            {
                string gender = Console.ReadLine();
                if (patient.SetGender(gender.ToLower(), out errorMessage))
                {
                    break;
                }
                Console.WriteLine($"Error: {errorMessage}");
                Console.WriteLine("--------------------");
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
                if (patient.SetSymptomCode(symptomCode.ToLower(), out errorMessage))
                {
                    break;
                }
                Console.WriteLine($"Error: {errorMessage}");
                Console.WriteLine("--------------------");
                Console.Write("Enter the symptom code from the above list (S1, S2, or S3): ");
            } while (true);

            Console.WriteLine("Your prescription based on your age, symptoms, and medical history:");
            Prescription prescription = medBot.PrescribeMedication(patient.GetSymptom(), patient.GetAge());
            Console.Clear();
            Console.WriteLine("Prescription: " + prescription.MedicineName + " " + prescription.Dosage);
            Console.WriteLine("\nThank you for coming.");

            Console.ReadKey();
        }
    }

    public enum Symptom
    {
        Headache,
        SkinRashes,
        Dizziness,
        Unknown
    }

    public class Medicine
    {
        public string Name { get; set; }
        public Dictionary<int, string> DosageByAge { get; set; }
    }

    public class Prescription
    {
        public string MedicineName { get; set; }
        public string Dosage { get; set; }
    }

    class MedicalBot
    {
        private Dictionary<Symptom, Medicine> _medicines;

        public MedicalBot()
        {
            _medicines = new Dictionary<Symptom, Medicine>
            {
                { Symptom.Headache, new Medicine { Name = "Ibuprofen", DosageByAge = new Dictionary<int, string> { { 18, "400 mg" }, { 100, "800 mg" } } } },
                { Symptom.SkinRashes, new Medicine { Name = "Diphenhydramine", DosageByAge = new Dictionary<int, string> { { 18, "50 mg" }, { 100, "300 mg" } } } },
                { Symptom.Dizziness, new Medicine { Name = "Dimenhydrinate", DosageByAge = new Dictionary<int, string> { { 18, "50 mg" }, { 100, "400 mg" } } } },
                { Symptom.Unknown, new Medicine { Name = "Unknown", DosageByAge = new Dictionary<int, string>() } }
            };
        }

        public Prescription PrescribeMedication(Symptom symptom, int patientAge)
        {
            var medicine = _medicines[symptom];
            var dosage = medicine.DosageByAge.FirstOrDefault(d => d.Key >= patientAge).Value ?? "Unknown";
            return new Prescription { MedicineName = medicine.Name, Dosage = dosage };
        }

        public static string GetBotName()
        {
            return "Bob";
        }
    }

    class Patient
    {
        private string _name;
        private int _age;
        private string _gender;
        private string _medicalHistory;
        private string _symptomCode;

        public string GetName()
        {
            return _name;
        }
        public bool SetName(string name, out string errorMessage)
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


        public int GetAge()
        {
            return _age;
        }
        public bool SetAge(int age, out string errorMessage)
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


        public string GetGender()
        {
            return _gender;
        }
        public bool SetGender(string gender, out string errorMessage)
        {
            errorMessage = "";
            bool valid = false;
            if (gender == "male" || gender == "female" || gender == "other")
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


        public string GetMedicalHistory()
        {
            return _medicalHistory;
        }
        public void SetMedicalHistory(string medicalHistory)
        {
            _medicalHistory = medicalHistory;
        }


        public bool SetSymptomCode(string symptomCode, out string errorMessage)
        {
            errorMessage = "";
            bool valid = false;
            if (symptomCode == "s1" || symptomCode == "s2" || symptomCode == "s3")
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

        public Symptom GetSymptom()
        {
            switch (_symptomCode.ToLower())
            {
                case "s1":
                    return Symptom.Headache;
                case "s2":
                    return Symptom.SkinRashes;
                case "s3":
                    return Symptom.Dizziness;
                default:
                    return Symptom.Unknown;
            }
        }
    }
}
