using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace Enigma.Areas.Control.Controllers
{
    [Area("Control")]
    public class TranslationController : Controller
    {
        // The JSON string representing the dictionary entries
        string json = @"{Table1: [
  {
    ""OriginalWord"": ""a1"",
    ""Transcription"": ""[LeI]"",
    ""Type"": ""n. pl"",
    ""Translation"": ""a (1. slovo engleskog alfabeta) ocena »odličan«""
  },
  {
    ""OriginalWord"": ""a2"",
    ""Transcription"": ""[@]"",
    ""Type"": ""neodređeni član"",
    ""Translation"": ""neki, jedan""
  },
  {
    ""OriginalWord"": ""aback"",
    ""Transcription"": ""[@-Lb{k]"",
    ""Type"": ""adv."",
    ""Translation"": ""iznenađen, zapanjen""
  },
  {
    ""OriginalWord"": ""abacus"",
    ""Transcription"": ""[L{-b@-k@s]"",
    ""Type"": ""n. pl"",
    ""Translation"": ""abaci / abacuses računaljka""
  },
  {
    ""OriginalWord"": ""abaft"",
    ""Transcription"": ""[@-Lb{ft]"",
    ""Type"": ""prep."",
    ""Translation"": ""(naut.) prema krmi""
  },
  {
    ""OriginalWord"": ""abandon1"",
    ""Transcription"": ""[@-Lb{n-d@n]"",
    ""Type"": ""v."",
    ""Translation"": ""napustiti; odreći se""
  },
  {
    ""OriginalWord"": ""abandon2"",
    ""Transcription"": """",
    ""Type"": ""n."",
    ""Translation"": ""razuzdanost, raskalašnost""
  },
  {
    ""OriginalWord"": ""abandoned"",
    ""Transcription"": ""[@-Lb{n-d@nd]"",
    ""Type"": ""a. v. abandon"",
    ""Translation"": ""razuzdan, raskalašan, razvratan""
  },
  {
    ""OriginalWord"": ""abase"",
    ""Transcription"": ""[@-LbeIs]"",
    ""Type"": ""v."",
    ""Translation"": ""uniziti, poniziti""
  },
  {
    ""OriginalWord"": ""abasement"",
    ""Transcription"": ""[-LbeI-sm@nt]"",
    ""Type"": ""n."",
    ""Translation"": ""poniženje""
  },
  {
    ""OriginalWord"": ""abash"",
    ""Transcription"": ""[@-Lb{S]"",
    ""Type"": ""v."",
    ""Translation"": ""zbuniti""
  },
  {
    ""OriginalWord"": ""abashment"",
    ""Transcription"": ""[-m@nt]"",
    ""Type"": ""n."",
    ""Translation"": ""zbunjenost""
  },
  {
    ""OriginalWord"": ""abate"",
    ""Transcription"": ""[@-LbeIt]"",
    ""Type"": ""v."",
    ""Translation"": ""smanjiti, sniziti; smanjiti se, stišati se""
  },
  {
    ""OriginalWord"": ""abatement"",
    ""Transcription"": ""[@-LbeIt-m@nt]"",
    ""Type"": ""n."",
    ""Translation"": ""smanjenje, sniženje""
  },
  {
    ""OriginalWord"": ""abbess"",
    ""Transcription"": ""[L{-b@s]"",
    ""Type"": ""n."",
    ""Translation"": ""opatica, igumanija""
  },
  {
    ""OriginalWord"": ""abbey"",
    ""Transcription"": ""[L{-bi]"",
    ""Type"": ""n. pl"",
    ""Translation"": ""opatija, manastir""
  },
  {
    ""OriginalWord"": ""abbot"",
    ""Transcription"": ""[L{-b@t]"",
    ""Type"": ""n."",
    ""Translation"": ""opat, iguman""
  },
  {
    ""OriginalWord"": ""abbreviate"",
    ""Transcription"": ""[@-Lbri-vi-MeIt]"",
    ""Type"": ""v."",
    ""Translation"": ""skratiti""
  },
  {
    ""OriginalWord"": ""abbreviation"",
    ""Transcription"": ""[@-Mbri-vi-LeI-S@n]"",
    ""Type"": ""n."",
    ""Translation"": ""kraćenje; skraćenica, abrevijacija""
  },
  {
    ""OriginalWord"": ""ABC"",
    ""Transcription"": ""[MeI-(M)bi-Lsi]"",
    ""Type"": ""n. pl"",
    ""Translation"": ""azbuka; (fig.) abeceda""
  },
  {
    ""OriginalWord"": ""abdicate"",
    ""Transcription"": ""[L{b-dI-MkeIt]"",
    ""Type"": ""v."",
    ""Translation"": ""odreći se""
  },
  {
    ""OriginalWord"": ""abdication"",
    ""Transcription"": ""[M{b-dI-LkeI-S@n]"",
    ""Type"": ""n."",
    ""Translation"": ""odricanje, abdikacija""
  },
  {
    ""OriginalWord"": ""abdomen"",
    ""Transcription"": ""[L{b-d@-m@n]"",
    ""Type"": ""n."",
    ""Translation"": ""trbuh, abdomen; zadak (kod insekata)""
  },
  {
    ""OriginalWord"": ""abdominal"",
    ""Transcription"": ""[{b-LdA-m@-n@l]"",
    ""Type"": ""a."",
    ""Translation"": ""trbušni""
  },
  {
    ""OriginalWord"": ""abduct"",
    ""Transcription"": ""[{b-Ldökt]"",
    ""Type"": ""v."",
    ""Translation"": ""oteti""
  },
  {
    ""OriginalWord"": ""abduction"",
    ""Transcription"": ""[{b-Ldök-S@n]"",
    ""Type"": ""n."",
    ""Translation"": ""otmica""
  },
  {
    ""OriginalWord"": ""abductor"",
    ""Transcription"": ""[-Ldök-t@r]"",
    ""Type"": ""n."",
    ""Translation"": ""otmičar; (anat.) odmicač""
  },
  {
    ""OriginalWord"": ""abeam"",
    ""Transcription"": ""[@-Lbim]"",
    ""Type"": ""adv. / a."",
    ""Translation"": ""(naut.) popreko""
  },
  {
    ""OriginalWord"": ""aberrance"",
    ""Transcription"": ""[-@n(t)s]"",
    ""Type"": ""n."",
    ""Translation"": ""nenormalnost, odstupanje (od norme)""
  },
  {
    ""OriginalWord"": ""aberrant"",
    ""Transcription"": ""[{-LbEr-@nt]"",
    ""Type"": ""a."",
    ""Translation"": ""nenormalan""
  },
  {
    ""OriginalWord"": ""aberration"",
    ""Transcription"": ""[M{-b@-LreI-S@n]"",
    ""Type"": ""n."",
    ""Translation"": ""nenormalnost, odstupanje (od norme)""
  },
  {
    ""OriginalWord"": ""abhor"",
    ""Transcription"": ""[@b-LhOr]"",
    ""Type"": ""v."",
    ""Translation"": ""gnušati se""
  },
  {
    ""OriginalWord"": ""abhorrence"",
    ""Transcription"": ""[@b-LhOr-@n(t)s]"",
    ""Type"": ""n."",
    ""Translation"": ""gnušanje""
  },
  {
    ""OriginalWord"": ""abhorrent"",
    ""Transcription"": ""[-@nt]"",
    ""Type"": ""a."",
    ""Translation"": ""gnusan, odvratan; koji se gnuša""
  },
  {
    ""OriginalWord"": ""abide"",
    ""Transcription"": ""[@-LbaId]"",
    ""Type"": ""v."",
    ""Translation"": ""trpeti; povinovati se""
  },
  {
    ""OriginalWord"": ""abiding"",
    ""Transcription"": ""[@-LbaI-dIÎ]"",
    ""Type"": ""a."",
    ""Translation"": ""trajan, stalan""
  },
  {
    ""OriginalWord"": ""ability"",
    ""Transcription"": ""[@-LbI-l@-ti]"",
    ""Type"": ""n. pl"",
    ""Translation"": ""sposobnost, moć, veština""
  },
  {
    ""OriginalWord"": ""abiogenesis"",
    ""Transcription"": ""[MeI-MbaI-oU-LDE-n@-s@s]"",
    ""Type"": ""n."",
    ""Translation"": ""abiogeneza, samozačeće""
  },
  {
    ""OriginalWord"": ""abject"",
    ""Transcription"": ""[L{b-MDEkt]"",
    ""Type"": ""a."",
    ""Translation"": ""preziran, podao, ponizan; bedan""
  },
  {
    ""OriginalWord"": ""abjuration"",
    ""Transcription"": ""[M{b-D@-LreI-S@n]"",
    ""Type"": ""n."",
    ""Translation"": ""odricanje""
  },
  {
    ""OriginalWord"": ""abjure"",
    ""Transcription"": ""[{b-LDUr]"",
    ""Type"": ""v."",
    ""Translation"": ""odreći se""
  },
  {
    ""OriginalWord"": ""ablative"",
    ""Transcription"": ""[L{-bl@-tIv]"",
    ""Type"": ""a."",
    ""Translation"": ""ablativni""
  },
  {
    ""OriginalWord"": ""ablaze"",
    ""Transcription"": ""[@-LbleIz]"",
    ""Type"": ""a."",
    ""Translation"": ""u plamenu""
  },
  {
    ""OriginalWord"": ""able"",
    ""Transcription"": ""[L{b-@l]"",
    ""Type"": ""a."",
    ""Translation"": ""sposoban""
  },
  {
    ""OriginalWord"": ""ably"",
    ""Transcription"": ""[-bli]"",
    ""Type"": ""adv."",
    ""Translation"": ""vešto, sposobno""
  },
  {
    ""OriginalWord"": ""abnegation"",
    ""Transcription"": ""[M{b-nI-LgeI-S@n]"",
    ""Type"": ""n."",
    ""Translation"": ""odricanje, samozatajenje""
  },
  {
    ""OriginalWord"": ""abnormal"",
    ""Transcription"": ""[@b-LnOr-m@l]"",
    ""Type"": ""a."",
    ""Translation"": ""nenormalan, nepravilan""
  }
]
}";
        public TranslationController()
        {
            
        }

        public IActionResult Translator(string? word)
        {
            

            DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(json);

            DataTable dataTable = dataSet.Tables["Table1"];

            Console.WriteLine(dataTable.Rows.Count);


            Entity entity = new();
            foreach (DataRow row in dataTable.Rows)
            {
                if (row["OriginalWord"].ToString().ToLower() == word)
                {
                    entity.OriginalWord = row["OriginalWord"].ToString();
                    entity.Transcription = row["Transcription"].ToString();
                    entity.Type = row["Type"].ToString();
                    entity.Translation = row["Translation"].ToString();

                }
                Console.WriteLine(row["OriginalWord"] + " - " + row["translation"]);
            }


            return View(entity);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TranslatorPost(string? word)
        {
            return Redirect("https://localhost:7279/Control/Translation/Translator?word=" + word);
        }
       
    }
    public class Entity
    {
        public string OriginalWord { get; set; }
        public string Transcription { get; set; }
        public string Type { get; set; }
        public string Translation { get; set; }
    }
}
