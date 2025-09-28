Gokart Időpontfoglaló Rendszer (PSZ Projekt)
Ez a konzolos alkalmazás a 'Patyi Fasza Gokartja' számára készült, hogy hatékonyan kezelje a versenyzői adatokat, és támogassa a csoportos időpontfoglalást egészen 20 főig.
A projekt célja volt egy funkcionális foglalási rendszer létrehozása, ahol a külsős csatlakozások és a foglaltság vizuális visszajelzése is lekezelésre kerül.

Fő funkciók áttekintése:
  A program a főmenüből érhető el, és a következőket teszi lehetővé:
  Versenyzők Listája (Menüpont 1): Megjeleníti a program által generált összes regisztrált versenyző adatait (név, születési idő, ID, e-mail).
  
  Időpontok Megjelenítése (Menüpont 2): Vizuálisan áttekinthetővé teszi az adott hónap foglaltságát egy egyszerű színkódolással.
  
  Foglalás / Átfoglalás / Csatlakozás (Menüpont 3): Ezen a ponton kezelhető a teljes foglalási logika, beleértve az új foglalást és a meglévő foglaláshoz való csatlakozást is.
  
  Kilépés (Menüpont 4): Bezárja a programot.

Vizuális visszajelzés a telítettségről:
  Az Időpontok Megjelenítése menüpontban egyértelmű visszajelzést kapsz az időpont telítettségéről, a maximális 20 fős keret figyelembevételével:

  Szín	Jelentés	Létszám
  
  Zöld	Szabad	Még nincs foglalás az időpontra.
  
  Sárga	Részben foglalt	Van már foglalás (pl. 8-19 főre), de lehet külsősként csatlakozni a 20 fős limit eléréséig.
  
  Piros	Telt ház	Az időpont teljesen betelt (20 fő). Nincs lehetőség csatlakozásra.

A Foglalási és Csatlakozási Logika:
  A 3. menüpont (Foglalás / Átfoglalás / Csatlakozás) mögötti logika kezeli a létszámkorlátokat és a rugalmasságot.

  1. Új Foglalás (U)
    Létszámkorlát: Új foglalás csak minimum 8 fő és maximum 20 fő között adható le. Ez biztosítja a csoportos események keretét.
    Konfliktuskezelés: Ha az adott időpont már foglalt, a rendszer hibaüzenetet ad, és nem engedélyezi az új foglalást.

  2. Külsős Csatlakozás (C)
    Ez a funkció lehetővé teszi, hogy egy versenyző meglévő, de sárgával jelölt időponthoz csatlakozzon
    A rendszer bekéri a csatlakozók létszámát.
    Kapacitásellenőrzés: Csak annyi fő csatlakozhat, amennyi a 20 fős maximális keretből még szabad.
    Sikeres csatlakozás esetén a foglalás létszáma azonnal frissül a dictionaryben.

  3. Átfoglalás
    Ha a versenyzőnek már van aktív foglalása (amit korábban ő maga hozott létre), a rendszer rákérdez, hogy szeretné-e törölni az előző foglalását, mielőtt újat adna le, vagy csatlakozna egy meglévőhöz.

Adatkezelés:
  A program a beolvasott és generált adatokat egyszerű, memóriában tárolt adatszerkezetekkel kezeli:
  Versenyzok: Az egyes regisztrált személyek adatait és egyedi ID-ját (pl. GO-patyifasza-20250915) tárolja.
  FoglalasAdat osztály: Ez a kulcsfontosságú segédosztály tárolja minden lefoglalt időponthoz az eredeti foglaló ID-ját és a jelenlegi összes létszámot. Ezzel a megoldással tudjuk nyomon követni a 20 fős keretet a csatlakozások után is.
  Ez a szerkezet teszi lehetővé a gyors ellenőrzéseket (telítettség, ID alapján történő keresés) és az egyszerű bővítést a jövőben.
