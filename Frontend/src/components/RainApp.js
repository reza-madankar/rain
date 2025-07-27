import React, { useState, useMemo } from 'react';
import { Layout, Typography, Space, Card, Button, Avatar } from 'antd';
import { UserOutlined, LogoutOutlined } from '@ant-design/icons';
import RainTable from './RainTable';
import AddRainModal from './AddRainModal';
import UserIdModal from './UserIdModal';
import useUserStore from '../store/userStore';

const { Header, Content } = Layout;
const { Title, Text } = Typography;

const RainApp = () => {
  const { userId, clearUserId } = useUserStore();
  const [isModalVisible, setIsModalVisible] = useState(false);

  const showModal = () => {
    setIsModalVisible(true);
  };

  const handleModalCancel = () => {
    setIsModalVisible(false);
  };

  const handleModalSuccess = () => {
    setIsModalVisible(false);
  };

  const handleLogout = () => {
    clearUserId();
  };

  return (
    <Layout style={{ minHeight: '100vh' }}>
      <Header style={{ background: '#fff', padding: '0 24px', display: 'flex', alignItems: 'center', justifyContent: 'space-between' }}>
        <Title level={3} style={{ margin: 0, color: '#1890ff' }}>
          Authodesk Assignment - MohammadReza Madankar
        </Title>
        {userId && (
          <Space>
            <Avatar icon={<UserOutlined />} />
            <Text strong>{userId}</Text>
            <Button 
              type="text" 
              icon={<LogoutOutlined />} 
              onClick={handleLogout}
              title="Change User ID"
            >
              Logout
            </Button>
          </Space>
        )}
      </Header>
      <Content style={{ padding: '24px' }}>
        {userId ? (
          <Card>
            <Space direction="vertical" size="large" style={{ width: '100%' }}>
              <RainTable 
                onAddNew={showModal}
              />
            </Space>
          </Card>
        ) : (
          <Card>
            <div style={{ textAlign: 'center', padding: '40px 0' }}>
              <UserOutlined style={{ fontSize: '48px', color: '#d9d9d9', marginBottom: '16px' }} />
              <Title level={4} type="secondary">
                Please enter your User ID to continue
              </Title>
              <Text type="secondary">
                Your User ID is required to access rain data
              </Text>
            </div>
          </Card>
        )}

        <AddRainModal
          visible={isModalVisible}
          onCancel={handleModalCancel}
          onSuccess={handleModalSuccess}
        />

        <UserIdModal />
      </Content>
    </Layout>
  );
};

export default RainApp; 