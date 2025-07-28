import React, { useState, useMemo } from 'react';
import { useQuery } from '@tanstack/react-query';
import {
  Table,
  Button,
  Space,
  Select,
  Card,
  Typography,
  message,
  Row,
  Col
} from 'antd';
import dayjs from 'dayjs';
import { PlusOutlined } from '@ant-design/icons';
import axios from '../helper/axios';

import useUserStore from '../store/userStore';

import sunImg from '../resources/images/sun.png';
import rainImg from '../resources/images/rain.png';

const { Title } = Typography;
const { Option } = Select;

const RainTable = ({ onAddNew }) => {
  const [currentPage, setCurrentPage] = useState(1);
  const [isRainFilter, setIsRainFilter] = useState(null);

  const userId = useUserStore((state) => state.userId);

console.log(userId)

  const {
    data,
    isLoading,
    error
  } = useQuery({
    queryKey: ['rainData', currentPage, isRainFilter, userId],
    queryFn: async () => {
      let isRainParam = isRainFilter;
      if (isRainFilter === "null" || isRainFilter === null) {
        isRainParam = null;
      } else if (isRainFilter === "true") {
        isRainParam = true;
      } else if (isRainFilter === "false") {
        isRainParam = false;
      }
      const params = {
        page: currentPage,
        isRain: isRainParam
      };
      const response = await axios.get('/data', { params });
      return response.data;
    },
    staleTime: 30000,
  });

  const tableData = useMemo(() => {
    if (!data?.rain) return [];


    return data.rain.map((item, index) => {
      const d = dayjs(item.timestamp);
      
      return ({
      key: index,
      date: d.format('YYYY, dddd, MMMM D'),
      time: d.format('HH:mm:ss'), 
      rain: item.rain,
      rainValue: item.rain
    })});
  }, [data]);

  const columns = [
    {
      title: 'Date',
      dataIndex: 'date',
      key: 'date',
    },
    {
      title: 'Time',
      dataIndex: 'time',
      key: 'time',
    },
    {
      title: 'Rain',
      dataIndex: 'rain',
      key: 'rain',
      filters: [
        { text: <span><img src={rainImg} alt="Rain" style={{ width: 20, verticalAlign: 'middle' }} /> Rain</span>, value: true },
        { text: <span><img src={sunImg} alt="No Rain" style={{ width: 20, verticalAlign: 'middle' }} /> No Rain</span>, value: false },
      ],
      onFilter: (value, record) => record.rain === value,
      render: (rain) =>
        rain ? (
          <img src={rainImg} alt="Rain" style={{ width: 28, verticalAlign: 'middle' }} />
        ) : (
          <img src={sunImg} alt="No Rain" style={{ width: 28, verticalAlign: 'middle' }} />
        ),
    },
  ];

  const handleTableChange = (pagination) => {
    setCurrentPage(pagination.current);
  };

  const handleFilterChange = (value) => {
    setIsRainFilter(value === "null" ? null : value);
    setCurrentPage(1);
  };

  if (error) {
    message.error('Failed to fetch rain data. Please try again.');
  }

  return (
    <Card>
      <Space direction="vertical" size="middle" style={{ width: '100%' }}>
        <Row gutter={[16, 16]} align="middle">
          <Col xs={24} sm={12} md={8}>
            <Title level={4}>Rain Data</Title>
          </Col>
          <Col xs={24} sm={12} md={16} style={{ textAlign: 'right' }}>
            <Space wrap>
              <Select
                value={isRainFilter === null ? "null" : isRainFilter}
                onChange={handleFilterChange}
                style={{ width: 120 }}
                placeholder="Filter by rain"
              >
                <Option value="null">All</Option>
                <Option value="true">
                  <span>
                    <img src={rainImg} alt="Rain" style={{ width: 18, verticalAlign: 'middle', marginRight: 4 }} />
                    Rain
                  </span>
                </Option>
                <Option value="false">
                  <span>
                    <img src={sunImg} alt="No Rain" style={{ width: 18, verticalAlign: 'middle', marginRight: 4 }} />
                    No Rain
                  </span>
                </Option>
              </Select>
              <Button
                type="primary"
                icon={<PlusOutlined />}
                onClick={onAddNew}
              >
                Add New Record
              </Button>
            </Space>
          </Col>
        </Row>

        <Table
          columns={columns}
          dataSource={tableData}
          loading={isLoading}
          pagination={{
            current: currentPage,
            total: data?.totalRecords || 0,
            showQuickJumper: true,
            showTotal: (total, range) =>
              `${range[0]}-${range[1]} of ${total} items`,
          }}
          onChange={handleTableChange}
          rowKey="key"
        />
      </Space>
    </Card>
  );
};

export default RainTable; 