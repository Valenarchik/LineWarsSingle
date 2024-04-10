using System;
using DataStructures;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    public class GameRoot : DontDestroyOnLoadSingleton<GameRoot>
    {
        [Header("Draw")] 
        [SerializeField] private DrawHelperInstance drawHelper;
        
        [Header("Storages")]
        [SerializeField] private DeckCardsScriptableStorage cardsDatabase;
        [SerializeField] private MissionsScriptableStorage missionsStorage;
        [SerializeField] private BlessingStorage blessingStorage;
        [SerializeField] private PromoCodeContainer promoCodeContainer;

        [Header("Controllers")]
        [SerializeField] private DecksController decksController;
        [SerializeField] private CompaniesController companiesController;
        [SerializeField] private UserInfoController userController;
        [SerializeField] private LootBoxController lootBoxController;
        [SerializeField] private BlessingsController blessingsController;
        [SerializeField] private Store store;
        [SerializeField] private CardUpgrader cardUpgrader;
        
        [Header("Providers")]
        [SerializeField] private ProviderBase yandexGameProvider;
        
        [Header("Api")]
        [SerializeField] private SDKAdapter ruStoreAdapter;
        [SerializeField] private SDKAdapter yandexGameSdkAdapter;
        [SerializeField] private SDKAdapter fakeSDKAdapter;

        [Space]
        [SerializeField] private Platform platform;
        
        [Header("Debug")]
        [SerializeField] private bool logged = true;
        
        private IProvider<Deck> deckProvider;
        private IProvider<MissionInfo> missionInfoProvider;
        private IProvider<UserInfo> userInfoProvider;
        private IGetter<DateTime> timeGetter;
        private IGetter<DateTime> reusableTimeGetter;
        private SDKAdapter sdkAdapter;

        public IStorage<int, DeckCard> CardsDatabase => GameStartedLog() ? cardsDatabase : null;
        public IStorage<int, MissionData> MissionsStorage => GameStartedLog() ? missionsStorage : null;
        public IStorage<BlessingId, BaseBlessing> BlessingStorage => GameStartedLog() ? blessingStorage: null;
        public IGetter<DateTime> TimeGetter => GameStartedLog() ? timeGetter : null;
        public IGetter<DateTime> ReusableTimeGetter => GameStartedLog() ? reusableTimeGetter : null;
        
        public DecksController DecksController => GameStartedLog() ? decksController : null;
        public CompaniesController CompaniesController => GameStartedLog() ? companiesController : null;
        public UserInfoController UserController =>  GameStartedLog() ? userController : null;
        public LootBoxController LootBoxController =>  GameStartedLog() ? lootBoxController : null;
        public SDKAdapter SdkAdapter => GameStartedLog() ? sdkAdapter : null;

        public BlessingsController BlessingsController =>  GameStartedLog() ? blessingsController : null;
        public Store Store => GameStartedLog() ? store : null;
        public CardUpgrader CardUpgrader => GameStartedLog() ? cardUpgrader: null;
        
        public DrawHelperInstance DrawHelper => drawHelper;

        
        private bool fieldsValidated;
        public bool GameReady { get; private set; }
        public event Action OnGameReady;
        

        protected override void OnAwake()
        {
            DebugUtility.Logged = logged;
            GameVariables.Initialize();
            fieldsValidated = ValidateFields();
            
            sdkAdapter = platform switch
            {
                Platform.Local => fakeSDKAdapter,
                Platform.YandexGame => yandexGameSdkAdapter,
                Platform.RuStore => ruStoreAdapter,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            sdkAdapter.Initialize(promoCodeContainer.promoCodes);
        }

        public void StartGame()
        {
            if (!fieldsValidated)
            {
                DebugUtility.LogError("Cant start game because fields error!");
                return;
            }
            
            if (GameReady)
            {
                DebugUtility.LogError("Game is initialized!");
                return;
            }
            

            timeGetter = new GetLocalTime();
            reusableTimeGetter = new GetLocalTime();
            
            InitializeProviders();
            
            companiesController.Initialize(missionInfoProvider, missionsStorage);
            userController.Initialize(userInfoProvider, cardsDatabase, blessingStorage);
            decksController.Initialize(deckProvider, userController);
            store.Initialize(timeGetter, cardsDatabase, blessingStorage, userController);
            blessingsController.Initialize(userController, userController, blessingStorage);
            cardUpgrader.Initialize(userController, cardsDatabase);
            InitializeLootBoxController();
            
            GameReady = true;
            OnGameReady?.Invoke();
            OnGameReady = null;
        }

        private void InitializeProviders()
        {
            switch (platform)
            {
                case Platform.Local:
                {
                    InitializeJsonProviders();
                    break;
                }
                case Platform.YandexGame:
                {
                    InitializeYandexGameProvider();
                    break;
                }
                case Platform.RuStore:
                    InitializeJsonProviders();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private void InitializeJsonProviders()
        {
            deckProvider = new Provider<Deck>(
                new SaverConvertDecorator<Deck, DeckInfo>(
                    new JsonFileSaver<DeckInfo>(),
                    new DeckToInfoConverter(cardsDatabase.ValueToId)
                ),
                new DownloaderConvertDecorator<Deck, DeckInfo>(
                    new JsonFileLoader<DeckInfo>(),
                    new DeckInfoToDeckConverter(cardsDatabase.IdToValue)
                ),
                new AllDownloaderConvertDecorator<Deck, DeckInfo>(
                    new JsonFileAllDownloader<DeckInfo>(),
                    new DeckInfoToDeckConverter(cardsDatabase.IdToValue))
            );

            userInfoProvider = new Provider<UserInfo>(
                new JsonFileSaver<UserInfo>(),
                new JsonFileLoader<UserInfo>(),
                new JsonFileAllDownloader<UserInfo>()
            );

            missionInfoProvider = new Provider<MissionInfo>(
                new JsonFileSaver<MissionInfo>(),
                new JsonFileLoader<MissionInfo>(),
                new JsonFileAllDownloader<MissionInfo>()
            );
        }
        private void InitializeYandexGameProvider()
        {
            deckProvider = new Provider<Deck>(
                new SaverConvertDecorator<Deck, DeckInfo>(
                    yandexGameProvider,
                    new DeckToInfoConverter(cardsDatabase.ValueToId)
                ),
                new DownloaderConvertDecorator<Deck, DeckInfo>(
                    yandexGameProvider,
                    new DeckInfoToDeckConverter(cardsDatabase.IdToValue)
                ),
                new AllDownloaderConvertDecorator<Deck, DeckInfo>(
                    yandexGameProvider,
                    new DeckInfoToDeckConverter(cardsDatabase.IdToValue))
            );


            userInfoProvider = yandexGameProvider;
            missionInfoProvider = yandexGameProvider;
        }
        private void InitializeLootBoxController()
        {
            var lootBoxOpenerFabric = new ClientLootBoxOpenerFabric(cardsDatabase, blessingStorage);
            var dropConverter = new DuplicateEreaserDropConverter(userController.UserInfo, cardsDatabase);
            lootBoxController.Initialize(
                userController.UserInfo, 
                lootBoxOpenerFabric, 
                dropConverter, 
                userController);

        }

        private bool ValidateFields()
        {
            if (cardsDatabase == null)
                return DebugUtility.LogError($"{nameof(cardsDatabase)} is null on {name}!", gameObject);
            if (decksController == null)
                return DebugUtility.LogError($"{nameof(decksController)} is null on {name}!", gameObject);
            return true;
        }

        private bool GameStartedLog()
        {
            if (!GameReady)
                DebugUtility.LogError("Game has not started!");
            return GameReady;
        }
    }

    public enum Platform
    {
        Local,
        YandexGame,
        RuStore
    }
}