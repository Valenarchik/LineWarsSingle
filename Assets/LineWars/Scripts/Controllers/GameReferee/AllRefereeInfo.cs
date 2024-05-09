using UnityEngine;

namespace LineWars
{
    [CreateAssetMenu]
    public class AllRefereeInfo: ScriptableObject
    {
        [SerializeField] private GameRefereeInfo captureThePoints;
        [SerializeField] private GameRefereeInfo newDomination;
        [SerializeField] private GameRefereeInfo kingOfMountain;
        [SerializeField] private GameRefereeInfo mapCapture;
        [SerializeField] private GameRefereeInfo siege;
        [SerializeField] private GameRefereeInfo wallToWall;

        public GameRefereeInfo CaptureThePoints => captureThePoints;

        public GameRefereeInfo NewDomination => newDomination;

        public GameRefereeInfo KingOfMountain => kingOfMountain;

        public GameRefereeInfo MapCapture => mapCapture;

        public GameRefereeInfo Siege => siege;

        public GameRefereeInfo WallToWall => wallToWall;
    }
}