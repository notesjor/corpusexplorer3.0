#region

using System.Linq;
using System.Text;

#endregion

namespace CorpusExplorer.Sdk._3rdParty.Commercial.Metaphone3
{
  public class Metaphone3Deutsch
  {
    /**
     * Default maximum length of encoded key.
     */
    private const int DEFAULT_MAX_KEY_LENGTH = 8;

    /**
     * Running copy of primary key.
     */
    private readonly StringBuilder m_primary;

    /**
     * Running copy of secondary key.
     */
    private readonly StringBuilder m_secondary;

    /**
     * Default size of key storage allocation
     */
    private readonly int MAX_KEY_ALLOCATION = 32;

    /** 
   * Index of character in m_inWord currently being encoded.
   */
    private int m_current;

    /** 
   * Flag whether or not to encode consonants as exactly as possible.
   */
    private bool m_encodeExact;

    /**
     * Flag whether or not to encode non-initial vowels.
     */
    private bool m_encodeVowels;

    /**
   * Internal copy of word to be encoded, allocated separately from string
   * pointed to in incoming parameter.
   */
    private string m_inWord;

    /**
     * Index of last character in m_inWord.
     */
    private int m_last;

    /** 
   * Length of word sent in to be encoded, as measured at beginning of
   * encoding.
   */
    private int m_length;

    /**
     * Length of encoded key string.
     */
    private int m_metaphLength;

    // //////////////////////////////////////////////////////////////////////////////
    // Metaphone3Deutsch class definition
    // //////////////////////////////////////////////////////////////////////////////

    /**
     * Constructor, default. This constructor is most convenient when encoding
     * more than one word at a time. New words to encode can be set using
     * SetWord(char *).
     */
    public Metaphone3Deutsch()
    {
      m_primary = new StringBuilder();
      m_secondary = new StringBuilder();

      m_metaphLength = DEFAULT_MAX_KEY_LENGTH;
      m_encodeVowels = false;
      m_encodeExact = false;
    }

    /**
     * Subscript safe .charAt()
     * 
     * @param at
     * index of character to access
     * @return null if index out of bounds, .charAt() otherwise
     */
    private char CharAt(int at) =>
      // check substring bounds
      at < 0
   || at > m_length - 1
        ? '\0'
        : m_inWord[at];

    /**
     * check for prepositional prefixes and other combining prefixes such as
     * // or ge- ver- be- (and not 'beste', 'bestie') vor- zer- fruh- früh- auf- an- unter- um- aus- (and not 'australien')
     * // ?ein- wider- ?weider- zu- zer- voll- ab- fahr- buch- ein- un-
     * @return
     */
    private bool combiningPrefix() =>
      StringAt(m_current - 2, 2, "GE", "BE", "AN", "UM", "ZU", "AB", "UN", "")
   && !StringAt(m_current - 2, 5, "BESTE", "BESTI", "")
   || StringAt(m_current - 3, 3, "VER", "VOR", "ZER", "AUF", "AUS", "")
   && !StringAt(m_current - 2, 10, "AUSTRALIEN", "")
   || StringAt(m_current - 4, 4, "FRUH", "FRÜH", "FAHR", "BUCH", "VOLL", "")
   || StringAt(m_current - 5, 5, "WIDER", "UNTER", "");

    public string Encode(string word)
    {
      SetWord(word);
      Encode();
      return GetMetaph();
    }

    /**
     * Encodes input string to one or two key values according to Metaphone 3
     * rules.
     */
    public void Encode()
    {
      m_current = 0;

      m_length = m_inWord.Length;
      if (m_length < 1)
        return;

      // zero based index
      m_last = m_length - 1;

      m_primary.Length = 0;
      m_secondary.Length = 0;

      // /////////main loop//////////////////////////
      while (!(m_primary.Length   > m_metaphLength)
          && !(m_secondary.Length > m_metaphLength))
      {
        if (m_current >= m_length)
          break;

        switch (CharAt(m_current))
        {
          case 'A':
          case 'Ä':
          case 'E':
          case 'I':
          case 'O':
          case 'Ö':
          case 'U':
          case 'Ü':
          case 'Y':

            Encode_Vowels();
            break;

          case 'B':

            Encode_B();
            break;

          case 'ß':
          case 'Ç':

            MetaphAdd("S");
            m_current++;
            break;

          case 'C':

            Encode_C();
            break;

          case 'D':

            Encode_D();
            break;

          case 'F':

            Encode_F();
            break;

          case 'G':

            Encode_G();
            break;

          case 'H':

            Encode_H();
            break;

          case 'J':

            Encode_J();
            break;

          case 'K':

            Encode_K();
            break;

          case 'L':

            Encode_L();
            break;

          case 'M':

            Encode_M();
            break;

          case 'N':

            Encode_N();
            break;

          case 'P':

            Encode_P();
            break;

          case 'Q':

            Encode_Q();
            break;

          case 'R':

            Encode_R();
            break;

          case 'S':

            Encode_S();
            break;

          case 'T':

            Encode_T();
            break;

          case 'V':

            Encode_V();
            break;

          case 'W':

            Encode_W();
            break;

          case 'X':

            Encode_X();
            break;

          case 'Z':

            Encode_Z();
            break;

          default:

            m_current++;
            break;
        }
      }

      // only give back m_metaphLength number of chars in m_metaph
      if (m_primary.Length > m_metaphLength)
        m_primary.Length = m_metaphLength;

      if (m_secondary.Length > m_metaphLength)
        m_secondary.Length = m_metaphLength;

      // it is possible for the two metaphs to be the same
      // after truncation. lose the second one if so
      if (m_primary.ToString().Equals(m_secondary.ToString()))
        m_secondary.Length = 0;
    }

    /**
     * Encodes 'B'
     * 
     * when the German consonant 'b' appears at the end of a word it is pronounced as a /p/.
     */
    private void Encode_B()
    {
      if (m_encodeExact)
        MetaphAdd(m_current == m_last ? "P" : "B");
      else
        MetaphAdd("P");

      m_current += CharAt(m_current + 1) == 'B'
                || CharAt(m_current + 1) == 'P'
                     ? 2
                     : 1;
    }

    /**
     * * Encodes 'C'
     * *
     * (examples) imported words where initial "ch-" is sh || ch
     * * 
     * Chef
     * Chance
     * Champagner
     * Chauffeur
     * Charme
     * Chaussee
     * Chauvinist
     * chartern
     * checken
     * Chat
     * Cheeseburger
     * 
     * The German letters 'chs' are pronounced 'ks' in 
     * a similar manner to the English words 'vixen' and 'oxen'
     */
    private void Encode_C()
    {
      if (StringAt(m_current, 3, "CHS", "")
       && CharAt(m_current + 3) != 'T')
      {
        MetaphAdd("KS");
        m_current += 3;
      }
      else if (StringAt(m_current, 4, "CHEF", "CHAT", "")
            || StringAt(m_current, 6, "CHANCE", "CHARME", "CHECKE", "")
            || StringAt(m_current, 8, "CHAUSSEE", "CHARTERN", "")
            || StringAt(m_current, 9, "CHAUFFEUR", "")
            || StringAt(m_current, 10, "CHAUVINIST", "CHAMPAGNER", "")
            || StringAt(m_current, 12, "CHEESEBURGER", ""))
      {
        // words from french and english are pronounced
        // as in the source language
        MetaphAdd("X");
        m_current += 2;
      }
      else if (StringAt(m_current, 2, "CH", ""))
      {
        // we will code both the german palatal 
        // and uvular fricatives using the same
        // symbol that we use for the english glottal fricative
        if (m_current > 0 && CharAt(m_current - 1) == 'I')
          MetaphAdd("H", "X");
        else
          MetaphAdd("H");
        m_current += 2;
      }
      else
      {
        // 'C' as
        if (IsFrontVowel(m_current + 1))
        {
          MetaphAdd("TS");
          m_current++;
        }
        else
        {
          MetaphAdd("K");
          m_current += CharAt(m_current + 1) == 'K'
                    || CharAt(m_current + 1) == 'C'
                         ? 2
                         : 1;
        }
      }
    }

    /**
     * Encode "-D-"
     * 
     But when the German consonant 'd' appears at the end of a word it is pronounced as a /t/
     (and a tip of the hat to the engineer who asked me why original metaphone "implements
     final de-voicing")
     Note that the German consonant cluster '-dt' is also pronounced as a /t/
     */
    private void Encode_D()
    {
      if (m_current != m_last
       && !StringAt(m_current, 2, "DT", ""))
        MetaphAdd(m_encodeExact ? "D" : "T");
      else
        MetaphAdd("T");

      m_current += CharAt(m_current + 1) == 'D'
                || CharAt(m_current + 1) == 'T'
                     ? 2
                     : 1;
    }

    /**
     * Encode "-F-"
     */
    private void Encode_F()
    {
      MetaphAdd("F");
      m_current++;
    }

    /**
     * * Encode "-G-"
     * * 
     * Note that the consonant sound in an -ig suffix is also pronounced as a front "ch".
     * *
     * But when the German consonant 'g' appears at the end of a word it hardens to a /k/
     * *
     * The 'g' consonant retains its original pronunciation in a number of words that 
     * have been imported from other languages, e.g., '-age'
     * Garage
     * Reportage
     * Etage
     * Passage
     * Blamage
     * Rage
     * Engagement
     * 'gi-'
     * Regie
     * (but on the other hand, 	Psychologie => PSAHALAGA (like 'psuykhologee'))
     * Regisseur
     * Regime
     * [from english]
     * Manager
     * Management
     * Teenager
     * [contrasting pairs]
     * Genie (j) but not genial (g)
     * Gelee but not Gel
     * Genre but not Gen
     */
    private void Encode_G()
    {
      if (m_current == m_last
       && StringAt(m_current - 1, 2, "IG", ""))
      {
        MetaphAdd("K", "H");
        m_current++;
        return;
      }

      if (m_current == m_last)
      {
        MetaphAdd("K");
        m_current++;
        return;
      }

      if (StringAt(0, 5, "GENIE", "GELEE", "GENRE", "")
       || StringAt(0, 4, "RAGE", "")
       || StringAt(0, 5, "ETAGE", "")
       || StringAt(0, 5, "REGIE", "")
       || StringAt(0, 6, "GARAGE", "") && m_current == 4
       || StringAt(0, 6, "REGIME", "")
       || StringAt(0, 7, "PASSAGE", "BLAMAGE", "")
       || StringAt(0, 7, "MANAGER", "")
       || StringAt(0, 8, "TEENAGER", "")
       || StringAt(0, 9, "REPORTAGE", "")
       || StringAt(0, 9, "REGISSEUR", "")
       || StringAt(0, 10, "MANAGEMENT", "ENGAGEMENT", "") && m_current == 4)
      {
        // words from french and english are pronounced
        // as in the source language
        MetaphAdd("J");
        m_current++;
        return;
      }

      MetaphAdd(m_encodeExact ? "G" : "K");

      m_current += CharAt(m_current + 1) == 'G'
                || CharAt(m_current + 1) == 'K'
                     ? 2
                     : 1;
    }

    /**
     * * Encode 'H'
     * * 
     * But the letter 'h' is not pronounced at 
     * all if it used merely to indicate that the preceding vowel is a long one, e.g.
     * stehen
     * gehen
     * fahren
     * Lehrer
     * *
     */
    private void Encode_H()
    {
      if (IsVowel(m_current + 1)
       && !StringAt(m_current - 1, 3, "EHE", "UHI", "UHE", ""))
        MetaphAdd("H");

      m_current++;
    }

    /**
     * Encode 'J'
     * 
     * 
     * pronounced as Y except for some words imported from english || french.
     */
    private void Encode_J()
    {
      if (Foreign_J(m_current))
      {
        MetaphAdd("J");
        m_current++;
      }
      else
      {
        // if not in a foreign word, 
        // 'J' is a vowel in german
        Encode_Vowels();
      }
    }

    /**
     * Encode 'K'
     */
    private void Encode_K()
    {
      MetaphAdd("K");

      m_current += CharAt(m_current + 1) == 'G'
                || CharAt(m_current + 1) == 'K'
                     ? 2
                     : 1;
    }


    /**
     * Encode 'L'
     */
    private void Encode_L()
    {
      MetaphAdd("L");

      m_current += CharAt(m_current + 1) == 'L' ? 2 : 1;
    }

    /**
     * Encode "-M-"
     */
    private void Encode_M()
    {
      MetaphAdd("M");

      m_current += CharAt(m_current + 1) == 'M' ? 2 : 1;
    }


    /**
     * Encode "-N-"
     */
    private void Encode_N()
    {
      MetaphAdd("N");

      m_current += CharAt(m_current + 1) == 'N' ? 2 : 1;
    }


    /**
     * Encode "-P-"
     * 
     * As in English, the letters 'ph' are pronounced as an /f/
     */
    private void Encode_P()
    {
      if (StringAt(m_current, 2, "PH", ""))
      {
        MetaphAdd("F");
        m_current++;
      }
      else
      {
        MetaphAdd("P");
      }

      m_current += CharAt(m_current + 1) == 'P'
                || CharAt(m_current + 1) == 'B'
                     ? 2
                     : 1;
    }


    /**
     * * Encode "-Q-"
     * * 
     * As in English, the German consonant 'q' is always followed by the letter 'u' 
     * when it is written. It is pronounced however in a very different manner. It should 
     * be spoken as a 'kv' sound, which is only familiar to us from words such as 'kvetch
     * 
     * except e.g. "Boutique"
     */
    private void Encode_Q()
    {
      MetaphAdd("K");

      if (StringAt(m_current, 2, "QU", "")
       && !(m_current == m_last - 2
         && StringAt(m_current, 3, "QUE", "")))
      {
        MetaphAdd(m_encodeExact ? "V" : "F");
        m_current += 2;
        return;
      }

      m_current += CharAt(m_current + 1) == 'Q'
                || CharAt(m_current + 1) == 'K'
                     ? 2
                     : 1;
    }

    /**
     * Encode "-R-"
     */
    private void Encode_R()
    {
      MetaphAdd("R");

      m_current += CharAt(m_current + 1) == 'R' ? 2 : 1;
    }

    /**
     * * Encode "-S-" and ß
     * * 
     * When it comes before a vowel, the German consonant 's' is pronounced like English 'z' 
     * in words such as 'zinc' || 'zoo', although the German sound is not as strongly voiced. 
     * Native English speakers often therefore experiences difficulties in distinguishing between 
     * the letters 's' and 'z', particularly when they occur at the start of a word.
     * 
     * But be careful of the pronunciation of the German word Ski! 
     * It is pronounced as "Shee", an alternate version of the word
     * 
     * "-sch-"
     * 
     * When they occur at the beginning of a word, the German consonant 
     * combinations 'sp' and 'st' are pronounced 'schp-' and 'scht-'
     * 
     * An exception to the above rule occurs when words beginning with 'sp' and 'st' are 
     * used as the final element in a compound noun. Although these sounds are positioned 
     * in the middle of the compound noun, the consonant combinations are still 
     * pronounced as they would be at the start of a word, i.e. as 'schp-' and 'scht-'. 
     * Hauptstadt
     * Hauptstraße
     * Ballspiel
     * Profisport
     * Straße
     * -Stein
     * -Stoff
     * -Spark
     * 
     * do NOT skip 'z's after s since 'z' is pronounced with a 't'
     */
    private void Encode_S()
    {
      // 'SKI': special case - not pronounced
      // at all the way we do in english
      if (StringAt(0, 3, "SKI", ""))
      {
        MetaphAdd("X");
        m_current += 2;
        return;
      }

      if (StringAt(m_current, 3, "SCH", ""))
      {
        MetaphAdd("X");
        m_current += 3;
        return;
      }

      if ((m_current == 0
        || combiningPrefix())
       && StringAt(m_current, 2, "SP", "ST", ""))
        MetaphAdd("X");
      else if (StringAt(
                        m_current,
                        5,
                        "STADT",
                        "STRAß",
                        "SPIEL",
                        "SPORT",
                        "STEIN",
                        "STOFF",
                        "SPARK",
                        "STÜCK",
                        "STEIG",
                        "STELL",
                        "STECK",
                        "STIMM",
                        "STURZ",
                        "STÜRZ",
                        "STUDI",
                        "STEHE",
                        "STRASS",
                        "STUFE",
                        "STOCK",
                        "STAND",
                        "SPRUC",
                        "SPRIN",
                        "SPAZI",
                        "SPÄNN",
                        "STURM",
                        ""))
        MetaphAdd("X");
      else
        MetaphAdd("S");

      m_current += CharAt(m_current + 1) == 'S' ? 2 : 1;
    }

    /**
     * Encode "-T-"
     * 
     "-tsch-" => 'X'
     '-tz' is also pronounced in the same way as English 'ts'.
     (remember to eat the 'z')
     */
    private void Encode_T()
    {
      //tsch, tzsch
      if (StringAt(m_current, 4, "TSCH", ""))
      {
        MetaphAdd("X");
        m_current += 4;
        return;
      }

      if (StringAt(m_current, 5, "TZSCH", ""))
      {
        MetaphAdd("X");
        m_current += 5;
        return;
      }

      if (StringAt(m_current, 2, "TZ", ""))
      {
        MetaphAdd("TS");
        m_current += 2;
        return;
      }

      MetaphAdd("T");

      m_current += CharAt(m_current + 1) == 'T'
                || CharAt(m_current + 1) == 'D'
                || CharAt(m_current + 1) == 'H'
                     ? 2
                     : 1;
    }

    /**
     * * Encode "-V-"
     * * 
     * In most instances the German consonant 'v' is pronounced 
     * in the same way as the letter 'f' 
     * 
     * In some German words however, the letter 'v' is pronounced as the phoneme /v/. 
     * These words include a number of items that are used very frequently in German. 
     * Klavier
     * Vase
     * November
     * Universität
     * privat
     * Verb
     */
    private void Encode_V()
    {
      if (m_encodeExact
       && (StringAt(m_current, 4, "VERB", "VASE", "")
        || StringAt(m_current - 3, 6, "PRIVAT", "")
        || StringAt(m_current - 3, 7, "KLAVIER", "")
        || StringAt(m_current - 2, 8, "NOVEMBER", "")
        || StringAt(m_current - 3, 11, "UNIVERSITÄT", "")))
        MetaphAdd("V");
      else
        MetaphAdd("F");

      m_current += CharAt(m_current + 1) == 'F'
                || CharAt(m_current + 1) == 'V'
                || CharAt(m_current + 1) == 'W'
                     ? 2
                     : 1;
    }

    /**
     * Encodes all initial vowels to A.
     * 
     * Encodes non-initial vowels to A if m_encodeVowels is true
     */
    private void Encode_Vowels()
    {
      if (m_current == 0)
        MetaphAdd("A");
      else if (m_encodeVowels)
        MetaphAdd("A");

      m_current = SkipVowels(m_current);
    }

    /**
     * Encode "-W-"
     */
    private void Encode_W()
    {
      MetaphAdd(m_encodeExact ? "V" : "F");

      m_current += CharAt(m_current + 1) == 'F'
                || CharAt(m_current + 1) == 'V'
                || CharAt(m_current + 1) == 'W'
                     ? 2
                     : 1;
    }

    /**
     * Encode "-X-"
     * 
     The letter x  occurs almost exclusively in loanwords. 
     Native German words that are now pronounced with a /ks/ sound are usually 
     written using chs or cks. Some 
     exceptions do occur, though, like in Hexe (witch), Nixe (mermaid) and Axt (axe).
     */
    private void Encode_X()
    {
      MetaphAdd("KS");

      m_current += CharAt(m_current + 1) == 'S' ? 2 : 1;
    }

    /**
     * Encode "-Z-"
     * 
     The German consonant 'z' is pronounced in the same manner as the English 'ts'
     */
    private void Encode_Z()
    {
      if (StringAt(m_current - 2, 4, "JAZZ", ""))
      {
        MetaphAdd("S", "Z");
        m_current += 2;
        return;
      }

      //zsch
      if (StringAt(m_current, 4, "ZSCH", ""))
      {
        MetaphAdd("X");
        m_current += 4;
        return;
      }

      MetaphAdd("TS");

      m_current += CharAt(m_current + 1) == 'S' ? 2 : 1;
    }

    /**
     * Detect words with 'J' from French and English that
     * are pronounced according to language of origin
     * 
     * Job
     * Jazz
     * Jet
     * Jeep
     * Jeans
     * Jumper
     * Jackpot
     * joggen
     * Journalist
     * Journal
     * Journalistin
     * Jalousie
     */
    private bool Foreign_J(int current) =>
      StringAt(current, 3, "JOB", "JET", "")
   && current + 2 == m_last
   || StringAt(current, 4, "JAZZ", "JEEP", "")
   || StringAt(current, 5, "JEANS", "")
   || StringAt(current, 6, "JUMPER", "JOGGEN", "")
   || StringAt(current, 7, "JACKPOT", "JOURNAL", "")
   || StringAt(current, 8, "JALOUSIE", "");

    /**
   * Retrieves alternate encoded key, if any.
   * 
   * @return a character pointer to the alternate encoded key
   */
    public string GetAlternateMetaph()
    {
      var secondary = m_secondary.ToString();
      return secondary;
    }

    /**
     * Retrieves setting determining whether or not consonants will be encoded
     * "exactly".
     * 
     * @return true if the Metaphone3 object has been set to encode "exactly",
     * false if not.
     */
    public bool GetEncodeExact() => m_encodeExact;

    /**
     * Retrieves setting determining whether or not non-initial vowels will be
     * encoded.
     * 
     * @return true if the Metaphone3 object has been set to encode non-initial
     * vowels, false if not.
     */
    public bool GetEncodeVowels() => m_encodeVowels;

    /**
   * Retrieves maximum number of characters currently allocated for encoded
   * key.
   * 
   * @return short integer representing the length allowed for the key.
   */
    public int GetKeyLength() => m_metaphLength;

    /**
     * Retrieves maximum number of characters allowed for encoded key.
     * 
     * @return short integer representing the length of allocated storage for
     * the key.
     */
    public int GetMaximumKeyLength() => MAX_KEY_ALLOCATION;

    /**
   * Retrieves primary encoded key.
   * 
   * @return a character pointer to the primary encoded key
   */
    public string GetMetaph()
    {
      var primary = m_primary.ToString();
      return primary;
    }

    /**
     * Tests if character is a "front" vowel
     * 
     * @param at position of character to be tested in string to be encoded
     * @return true if character is a vowel, false if not
     */
    private bool IsFrontVowel(int at)
    {
      if (at < 0 || at >= m_length)
        return false;

      var it = CharAt(at);

      return it == 'E'
          || it == 'I';
    }

    /**
     * Tests if character is a vowel
     * 
     * @param inChar
     * character to be tested in string to be encoded
     * @return true if character is a vowel, false if not
     */
    private bool IsVowel(char inChar) =>
      inChar == 'A' || inChar == 'E' || inChar == 'I'
   || inChar == 'O' || inChar == 'U' || inChar == 'Y'
   || inChar == 'À' || inChar == 'Á' || inChar == 'Â'
   || inChar == 'Ã' || inChar == 'Ä' || inChar == 'Å'
   || inChar == 'Æ' || inChar == 'È' || inChar == 'É'
   || inChar == 'Ê' || inChar == 'Ë' || inChar == 'Ì'
   || inChar == 'Í' || inChar == 'Î' || inChar == 'Ï'
   || inChar == 'Ò' || inChar == 'Ó' || inChar == 'Ô'
   || inChar == 'Õ' || inChar == 'Ö' || inChar == 'Œ'
   || inChar == 'Ø' || inChar == 'Ù' || inChar == 'Ú'
   || inChar == 'Û' || inChar == 'Ü' || inChar == 'Ý'
   || inChar == 'Ÿ';

    /**
     * Tests if character in the input string is a vowel
     * 
     * @param at
     * position of character to be tested in string to be encoded
     * @return true if character is a vowel, false if not
     */
    private bool IsVowel(int at)
    {
      if (at < 0 || at >= m_length)
        return false;

      var it = CharAt(at);

      return it == 'J' ? !Foreign_J(at) : IsVowel(it);
    }

    /**
     * Adds an encoding character to the encoded key value string - one
     * parameter version.
     * 
     * @param main
     * primary encoding character to be added to encoded key string.
     */
    private void MetaphAdd(string inString)
    {
      if (!(inString.Equals("A")
         && m_primary.Length                > 0
         && m_primary[m_primary.Length - 1] == 'A'))
        m_primary.Append(inString);

      if (!(inString.Equals("A")
         && m_secondary.Length                  > 0
         && m_secondary[m_secondary.Length - 1] == 'A'))
        m_secondary.Append(inString);
    }

    /**
     * Adds an encoding character to the encoded key value string - two
     * parameter version
     * 
     * @param main
     * primary encoding character to be added to encoded key string
     * @param alt
     * alternative encoding character to be added to encoded
     * alternative key string
     */
    private void MetaphAdd(string main, string alt)
    {
      if (!(main.Equals("A")
         && m_primary.Length                > 0
         && m_primary[m_primary.Length - 1] == 'A'))
        m_primary.Append(main);

      if (!(alt.Equals("A")
         && m_secondary.Length                  > 0
         && m_secondary[m_secondary.Length - 1] == 'A'))
        if (alt.Length > 0)
          m_secondary.Append(alt);
    }

    /*
     * Map Multibyte characters to regular ASCII
     * 
     * ÃŸ => ß 
     * Ã¶ => Ö
     * Ã¤ => Ä
     * Ã¼ => Ü
     */

    private string remapMultiBytes(string inStr)
    {
      var fixedStr = inStr.Replace("ÃŸ", "ß");
      fixedStr = fixedStr.Replace("Ã¶", "Ö");
      fixedStr = fixedStr.Replace("Ã¤", "Ä");
      fixedStr = fixedStr.Replace("Ã¼", "Ü");
      return fixedStr;
    }

    /**
     * Sets flag that causes Metaphone3 to encode consonants as exactly as
     * possible. This does not include 'S' vs. 'Z', since americans will
     * pronounce 'S' at the at the end of many words as 'Z', nor does it include
     * "CH" vs. "SH". It does cause a distinction to be made between 'B' and
     * 'P', 'D' and 'T', 'G' and 'K', and 'V' and 'F'.
     * 
     * @param inEncodeExact
     * consonants to be encoded "exactly" if true, not if false.
     */
    public void SetEncodeExact(bool inEncodeExact)
    {
      m_encodeExact = inEncodeExact;
    }

    /**
     * Sets flag that causes Metaphone3 to encode non-initial vowels. However,
     * even if there are more than one vowel sound in a vowel sequence (i.e.
     * vowel diphthong, etc.), only one 'A' will be encoded before the next
     * consonant or the end of the word.
     * 
     * @param inEncodeVowels
     * Non-initial vowels encoded if true, not if false.
     */
    public void SetEncodeVowels(bool inEncodeVowels)
    {
      m_encodeVowels = inEncodeVowels;
    }

    /**
     * @ Param inKeyLength
     * Sets length allocated for output keys. If incoming number is greater than
     * maximum allowable length returned by GetMaximumKeyLength(), set key
     * length to maximum key length and return false; otherwise, set key length
     * to parameter value and return true.
     * 
     * @param inKeyLength
     * new length of key.
     * @return true if able to set key length to requested value.
     */
    public bool SetKeyLength(int inKeyLength)
    {
      if (inKeyLength < 1)
        inKeyLength = 1;

      if (inKeyLength > MAX_KEY_ALLOCATION)
      {
        m_metaphLength = MAX_KEY_ALLOCATION;
        return false;
      }

      m_metaphLength = inKeyLength;
      return true;
    }

    /**
     * Sets word to be encoded.
     * 
     * @param in
     * pointer to EXTERNALLY ALLOCATED char string of the word to be
     * encoded.
     */
    public void SetWord(string inString)
    {
      m_inWord = inString.ToUpper();
      m_inWord = remapMultiBytes(m_inWord);
      m_length = m_inWord.Length;
    }

    /**
     * Skips over vowels in a string. Has exceptions for skipping consonants
     * that will not be encoded.
     * 
     * @param at
     * position, in string to be encoded, of character to start
     * skipping from
     * 
     * @return position of next consonant in string to be encoded
     */
    private int SkipVowels(int at)
    {
      if (at < 0)
        return 0;

      if (at >= m_length)
        return m_length;

      while (IsVowel(at))
        at++;

      return at;
    }

    /**
   * Determines if one of the substrings sent in is the same as what is at the
   * specified position in the string being encoded.
   * 
   * @param start
   * @param length
   * @param compareStrings
   * @return
   */
    private bool StringAt(int start, int length, params string[] compareStrings)
    {
      // check substring bounds
      if (start < 0
       || start > m_length - 1
       || start + length   - 1 > m_length - 1)
        return false;

      var target = m_inWord.Substring(start, length);

      return compareStrings.Any(strFragment => target.Equals(strFragment));
    }
  }
}

//}