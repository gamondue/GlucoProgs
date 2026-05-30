graph TD
    A["👤 Utente apre StatisticsPage<br/>(dateFrom: 1 Gen, dateTo: 31 Gen)"] -->|Crea ViewModel| B["StatisticsPageViewModel<br/>ctor(dateFrom, dateTo)"]
    
    B -->|Carica settings| C["Load Meal Times<br/>Breakfast: 6:00-10:00<br/>Lunch: 11:00-15:00<br/>Dinner: 17:00-21:00"]
    
    C -->|CalculateAllStatistics| D["Three Calculation Branches"]
    
    D -->|Branch 1| E1["CalculateGlucoseStatistics"]
    D -->|Branch 2| E2["CalculateChoStatistics"]
    D -->|Branch 3| E3["CalculateInsulinStatistics"]
    
    %% GLUCOSE BRANCH
    E1 -->|1. Load data| F1["BL_GlucoseMeasurements<br/>GetGlucoseRecordsForStatistics<br/>dateFrom-dateTo"]
    F1 -->|2. Filter by hour range| F2["CalculateGlucoseStatistics<br/>startHour, endHour"]
    F2 -->|3. Compute| F3["Mean, StdDev, NSamples"]
    F3 -->|4. Format| F4["SetFromStatistics<br/>Mean: 105.5 mg/dL<br/>StdDev: 8.2 mg/dL<br/>Samples: 28"]
    F4 -->|Set Property| F5["GlucoseBreakfast<br/>GlucoseLunch<br/>GlucoseDinner<br/>GlucoseOther"]
    
    %% CHO BRANCH
    E2 -->|1. Load data| G1["BL_MealAndFood<br/>GetMeals<br/>dateFrom-dateTo"]
    G1 -->|2. Filter null values| G2["Convert to<br/>DateTime, CHO pairs<br/>List&lt;DateTime, double&gt;"]
    G2 -->|3. Group by day| G3["Daily CHO Sums<br/>Day1: 45g<br/>Day2: 52g<br/>Day3: 48g"]
    G3 -->|4. Calculate total stats| G4["GamonStatistics<br/>MeanAndStdDev<br/>List&lt;double&gt;"]
    G4 -->|Result| G5["Mean: 48.3 g/day<br/>StdDev: 3.2 g/day"]
    
    G2 -->|5. Create bands| G6["CreateMealTimeBands<br/>Breakfast: 6:00-10:00<br/>Lunch: 11:00-15:00<br/>Dinner: 17:00-21:00"]
    G6 -->|6. Apply method| G7["GamonStatistics<br/>MeansOfSumsInTimeBands<br/>choData, bands"]
    
    G7 -->|Daily grouping<br/>Band sum calculation| G8["Per Band Daily Sums<br/>Day1 Breakfast: 15g<br/>Day2 Breakfast: 18g<br/>Day1 Lunch: 20g<br/>Day2 Lunch: 22g"]
    
    G8 -->|Calculate mean/stddev<br/>across days| G9["Result Tuple<br/>Breakfast Mean: 16.5g<br/>Breakfast StdDev: 2.1g<br/>Breakfast Count: 2 days"]
    
    G9 -->|For each band| G10["SetChoBandStats<br/>bandStats[0] → ChoBreakfast<br/>bandStats[1] → ChoLunch<br/>bandStats[2] → ChoDinner<br/>bandStats[3] → ChoOther"]
    
    %% INSULIN BRANCH
    E3 -->|1. Load data| H1["BL_BolusesAndInjections<br/>GetInjectionsForStatistics<br/>dateFrom-dateTo"]
    H1 -->|2. Calculate TDD| H2["CalculateTddInsulin<br/>Total Daily Dose"]
    H2 -->|Result| H3["Mean: 42.5 U/day<br/>StdDev: 3.1 U/day"]
    
    H1 -->|3. Calculate per type| H4["CalculateTotalQuickInsulin<br/>CalculateTotalLongInsulin"]
    H4 -->|Results| H5["Quick: 28.3 U/day<br/>Long: 14.2 U/day"]
    
    H1 -->|4. Calculate per meal| H6["CalculateRapidActingBreakfast<br/>CalculateRapidActingLunch<br/>CalculateRapidActingDinner<br/>CalculateRapidActingOther"]
    H6 -->|Results| H7["Breakfast: 10.5 U<br/>Lunch: 8.2 U<br/>Dinner: 9.6 U<br/>Other: 0 U"]
    
    %% CONVERGENCE
    F5 -->|Property Changed| I["🔔 INotifyPropertyChanged"]
    G10 -->|Property Changed| I
    H7 -->|Property Changed| I
    
    %% UI BINDING
    I -->|Data Binding| J["StatisticsPage.xaml<br/>XAML Content"]
    
    J -->|Binding GlucoseBreakfast.Mean| K1["🖥️ Display<br/>Glucose Breakfast<br/>105.5 mg/dL ± 8.2"]
    J -->|Binding ChoBreakfast.Mean| K2["🖥️ Display<br/>CHO Breakfast<br/>16.5 g ± 2.1"]
    J -->|Binding InsulinBreakfast.Mean| K3["🖥️ Display<br/>Insulin Breakfast<br/>10.5 U/day"]
    
    K1 --> L["👁️ User sees<br/>Statistics"]
    K2 --> L
    K3 --> L
    
    style A fill:#e1f5ff
    style B fill:#fff3e0
    style C fill:#f3e5f5
    style D fill:#e8f5e9
    style E1 fill:#fce4ec
    style E2 fill:#e0f2f1
    style E3 fill:#ffe0b2
    style I fill:#c8e6c9
    style J fill:#bbdefb
    style L fill:#b2dfdb
    
    classDef data fill:#fff9c4,stroke:#f57f17,stroke-width:2px
    classDef method fill:#c5cae9,stroke:#3f51b5,stroke-width:2px
    classDef result fill:#a5d6a7,stroke:#388e3c,stroke-width:2px
    
    class G1,G2,G3,H1 data
    class G7,G4,H2,H4,H6 method
    class G9,G10,H3,H5,H7 result