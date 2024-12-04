namespace SubDrone {
    public class TreasurePiece : Treasure {

        public override void Interact() {
            base.Interact();

            // Audio effect

            // Remove object from the scene
            Destroy(gameObject);
        }

    }
}