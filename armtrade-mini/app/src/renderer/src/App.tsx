import { NavLink, Navigate, Route, Routes } from 'react-router-dom';
import Pos from './pages/Pos';
import Products from './pages/Products';
import Inventory from './pages/Inventory';
import Import from './pages/Import';
import Devices from './pages/Devices';

const nav = [
  { to: '/pos', label: 'Point of Sale', icon: '🛒' },
  { to: '/products', label: 'Products', icon: '📦' },
  { to: '/inventory', label: 'Inventory', icon: '📊' },
  { to: '/import', label: 'Import', icon: '⬆️' },
  { to: '/devices', label: 'Devices', icon: '🔌' },
];

export default function App() {
  return (
    <div className="layout">
      <aside className="sidebar">
        <div className="brand">
          ArmTrade <span>Mini</span>
        </div>
        <nav>
          {nav.map((n) => (
            <NavLink
              key={n.to}
              to={n.to}
              className={({ isActive }) => (isActive ? 'active' : '')}
            >
              <span className="ico">{n.icon}</span>
              {n.label}
            </NavLink>
          ))}
        </nav>
        <div className="foot">v0.1 · local SQLite</div>
      </aside>
      <main className="content">
        <Routes>
          <Route path="/" element={<Navigate to="/pos" replace />} />
          <Route path="/pos" element={<Pos />} />
          <Route path="/products" element={<Products />} />
          <Route path="/inventory" element={<Inventory />} />
          <Route path="/import" element={<Import />} />
          <Route path="/devices" element={<Devices />} />
        </Routes>
      </main>
    </div>
  );
}
