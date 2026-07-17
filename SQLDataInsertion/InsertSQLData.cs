using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;



namespace TemplarBot.SQLDataInsertion
{
    public class InsertSQLData
    {

        public async Task InsertQuoteAsync(string quote, string source) // This is the method that inserts a quote into the database. I will use this method in the InsertQuotes() command to insert all of the quotes from the Quotes() method into the database. After that, I will use the GetRandomQuoteAsync() method to retrieve a random quote from the database and display it in the Discord channel when the user types the corresponding command.
        {
            string connectionString = "Data Source=TYLERS-DOMAIN;Initial Catalog=SaintsDB;Integrated Security=True;Encrypt=False;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("INSERT INTO QuoteTable (QuoteText, Source) VALUES (@quote, @source)", conn))
            {
                cmd.Parameters.AddWithValue("@quote", quote);
                cmd.Parameters.AddWithValue("@source", source ?? (object)DBNull.Value);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }
        public List<(string Text, string Source)> Quotes() // This is the method that contains the quotes. I will use this method to insert the quotes into the database. After that, I will use the GetRandomQuoteAsync() method to retrieve a random quote from the database and display it in the Discord channel when the user types the corresponding command. [OBSOLETE]
        {
            return new List<(string, string)>
            {
                ("It is not that I want merely to be called a Christian, but to actually be one. Yes, if I prove to be one, then I can have the name...Come fire, cross, battling with wild beasts, wrenching of bones, mangling of limbs, crushing of my whole body, cruel tortures of the Devil--Only let me get to Jesus Christ!", "Ignatius of Antioch"),
                ("Pray without ceasing on behalf of other men...For cannot he that falls rise again?", "Ignatius of Antioch"),
                ("I would rather die for Christ than rule the whole earth.", "Ignatius of Antioch"),
                ("Do not have Jesus Christ on your lips, and the world in your hearts.", "Epistle to the Romans"),
                ("Permit me to imitate my suffering God... I am God's wheat and I shall be ground by the teeth of beasts, that I may become the pure bread of Christ.", "Ignatius of Antioch"),
                ("But pray unceasingly also for the rest of men, for they offer ground for hoping that they may be converted and win their way to God...", "Ignatius of Antioch"),
                ("Stand like a beaten anvil. It is the part of a good athlete to be bruised and to prevail.", "The Epistles of Ignatius and Polycarp"),
                ("Labour together with one another; strive in company together; run together; suffer together; sleep together; and awake together...", "St. Ignatius of Antioch: The Epistles"),
                ("I offer my life's breath for the sake of the Cross, which is a stumbling block to the unbelievers, but to us it is salvation and eternal life.", "Epistle to the Ephesians"),
                ("He who is devout to the Mother of God will certainly never be lost.", "Ignatius of Antioch"),
                ("It is better to keep silence and be something than to talk and be nothing.", "Epistle to the Ephesians"),
                ("The proper thing, then, is not merely to be styled Christians, but also to be such.", "Ignatius of Antioch"),
                ("Study, therefore, to be established in the doctrines of the Lord and the apostles...", "St. Ignatius of Antioch: The Epistles"),
                ("For where there is division and wrath, God doth not dwell.", "Epistle to the Philadelphians"),
                ("I am the wheat of God. May I be ground up by the teeth of the wild beasts until I become the fine bread of Christ...", "Ignatius of Antioch"),
                ("And do ye also pray for me, who have need of your love, along with the mercy of God...", "St. Ignatius of Antioch: The Epistles"),
                ("When he is hated by the world, he is beloved of God.", "St. Ignatius of Antioch: The Epistles"),
                ("Now I begin to be a disciple... only let me attain to Jesus Christ.", "St. Ignatius of Antioch: The Epistles"),
                ("The times call upon thee to pray...", "St. Ignatius of Antioch: The Epistles"),
                ("But if, as some that are without God say, that He only seemed to suffer...", "St. Ignatius of Antioch: The Epistles"),
                ("Jesus Christ, who was with the Father before the worlds and appeared at the end of time.", "Epistle to the Magnesians"),
                ("For if the Lord were in the body in appearance only...", "St. Ignatius of Antioch: The Epistles"),
                ("For what does it profit, if any one commends me, but blasphemes my Lord, not owning Him to be God incarnate?", "St. Ignatius of Antioch: The Epistles"),
                ("Go forward bravely. Fear nothing. Trust in God. All will be well.", "Joan of Arc"),
                ("I am not afraid… I was born to do this.”", "Joan of Arc"),
                ("There is no such thing as bad weather.All weather is good weather because it is God’s.", "Teresa of Avila"),
                ("Hearing nuns’ confessions is like being stoned to death with popcorn.", "Archbishop Fulton Sheen"),
                ("If honor were profitable, everyone would be honorable.", "Thomas More"),
                ("Ho, ho, ho! Merry Christmas!", "Nicholas of Myra, probably"),
                ("Every Saint has a past and every sinner has a future.", "Augustine"),
                ("Be who God meant you to be and you will set the world on fire.", "Catherine of Siena"),
                ("Go forth and set the world on fire.", "St. Ignatius of Loyola"),
                ("Teach us to give and not to count the cost.", "St. Ignatius of Loyola"),
                ("Love ought to show itself in deeds more than words.", "St. Ignatius of Loyola"),
                ("For it is in giving that we receive.", "St. Francis of Assisi"),
                ("Love God, serve God, everything is in that.", "St. Clare of Assisi"),
                ("Pray, hope, and don't worry.", "St. Padre Pio"),
                ("Arm yourself with prayer rather than a sword; wear humility rather than fine clothes.", "St. Dominic"),
                ("Be who you were created to be, and you will set the world on fire.", "St. Catherine of Siena"),
                ("The gift of grace increases as the struggle increases.", "St. Rose of Lima"),
                ("The world offers you comfort; but you were not made for comfort, but greatness.", "St. Benedict"),
                ("I asked you and you did not listen. So I asked my God and He did listen.", "St. Scholastica"),
                ("I was no longer the center of my life and therefore I could see God in everything.", "Bede The Venerable"),
                ("Let us do our part and then God will do what He wills.", "St. Teresa of Avila"),
                ("The closer one approaches to God, the simpler one becomes.", "St. Therese of Liseaux"),
                ("In trial, turn to God in confidence. You will obtain strength, light, and knowledge.", "St. John of the Cross"),
                ("May the passionate Jesus Christ be always in our hearts.", "St. Paul of the Cross"),
                ("Lift yourself up to Him, who has lowered Himself for you.", "St. Gemma Galgani"),
                ("Our perfection does not consist in doing the extraordinary, but doing the ordinary well.", "St. Gabriel Possenti"),
                ("Whatever you do, think of the Glory of God as your main goal.", "St.John Bosco"),
                ("Nothing is so strong as gentleness; Nothing so gentle as real strength.", "St.Frances de Sales"),
                ("I can’t do big things, but I want to do everything for the Glory of God!", "St.Dominic Savio"),
            };
        }

        public List<(string Quote, string Source)> SaintQuotes() // This is the .txt insert method.
        {
            var lines = File.ReadAllLines("TextFileAccess/Quotes.txt");
            var quotes = new List<(string, string)>();

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                // Format: Quote text | Source
                var parts = line.Split('|');

                string text = parts[0].Trim();
                string source = parts.Length > 1 ? parts[1].Trim() : "Unknown";

                quotes.Add((text, source));
            }

            return quotes;
        }


        public async Task<(string Quote, string Source)> GetRandomQuoteAsync() // This is the method that retrieves a random quote from the database.
        {
            string connectionString = "Data Source=TYLERS-DOMAIN;Initial Catalog=SaintsDB;Integrated Security=True;Encrypt=False;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 QuoteText, Source FROM QuoteTable ORDER BY NEWID()", conn))
            {
                await conn.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        string quote = reader.GetString(0);
                        string source = reader.GetString(1);
                        return (quote, source);
                    }
                }
            }

            return ("No quotes could be found.", "Unknown");
        }

        public async Task WipeQuoteTableAsync() // This is the method that wipes the QuoteTable in the database.
        {
            string connectionString = "Data Source=TYLERS-DOMAIN;Initial Catalog=SaintsDB;Integrated Security=True;Encrypt=False;";
            string query = "DELETE FROM QuoteTable;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
