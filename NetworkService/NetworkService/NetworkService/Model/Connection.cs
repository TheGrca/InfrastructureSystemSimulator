using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Model
{
    public class Connection : BindableBase
    {
        private Entity _entity1;
        private Entity _entity2;

        public Entity Entity1
        {
            get => _entity1;
            set => SetProperty(ref _entity1, value);
        }

        public Entity Entity2
        {
            get => _entity2;
            set => SetProperty(ref _entity2, value);
        }

        public Connection(Entity entity1, Entity entity2)
        {
            _entity1 = entity1;
            _entity2 = entity2;
        }
    }
}
