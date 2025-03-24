# UnitTests
Unit, Integration and Feature tests

Dette projekt består af et simuleret API; dvs. der findes ingen egentlige endpoints, men strukturen er efterlignet til det formål at teste diverse dele af den struktur.

Jeg har forsøgt mig med at udarbejde små Unit Tests, som hver tester enormt små dele, generelt enestående funktioner, af programmet.

Alt hvad jeg ellers ville have kaldt en unit test, men som persister data gennem EF Cores InMemory-database har jeg kaldt Integration tests. Har ikke skrevet så mange af disse som jeg havde regnet med, da der er mange ting InMemory-databasen ikke er i stand til, kontra en 'rigtig' database. 

Til sidste Feature Tests, hvor jeg har haft fokus på 'Customer'-delen af en udvidet 'Hæveautomat'-opgave, og deres Creation-flow, inkl. med brug af JSON Web Tokens, som jeg aldrig fik brugt i vores store projekt.
